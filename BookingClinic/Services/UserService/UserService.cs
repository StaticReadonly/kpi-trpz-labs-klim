using BookingClinic.Data.Entities;
using BookingClinic.Data.Repositories.ClinicRepository;
using BookingClinic.Data.Repositories.SpecialityRepository;
using BookingClinic.Data.Repositories.UserRepository;
using BookingClinic.Services.Data.Doctor;
using BookingClinic.Services.Data.User;
using BookingClinic.Services.NotificationService.AdminNotificationService;
using Mapster;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace BookingClinic.Services.UserService
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly ISpecialityRepository _specialityRepository;
        private readonly IClinicRepository _clinicRepository;
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly IAdminAlertQueue _alertQueue;

        public UserService(
            IUserRepository userRepository,
            ISpecialityRepository specialityRepository,
            IClinicRepository clinicRepository,
            IWebHostEnvironment hostEnvironment,
            IAdminAlertQueue alertQueue)
        {
            _userRepository = userRepository;
            _specialityRepository = specialityRepository;
            _clinicRepository = clinicRepository;
            _hostEnvironment = hostEnvironment;
            _alertQueue = alertQueue;
        }

        private string GetPasswordHash(UserBase user, string password)
        {
            using var sha256 = SHA256.Create();
            
            var salt = user.Id.ToString() + user.Email + user.Phone;
            var saltedPassword = $"{password}:{salt}";
            var bytes = Encoding.UTF8.GetBytes(saltedPassword);
            var hash = sha256.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }

        private ClaimsPrincipal CreateClaimsPrincipal(UserBase user)
        {
            var claims = new Claim[]
            {
                new(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new(ClaimTypes.Role, user.Role)
            };

            var identity = new ClaimsIdentity(claims, "Cookie");
            return new ClaimsPrincipal(identity);
        }

        public async Task<ServiceResult<IEnumerable<SearchDoctorResDto>>> SearchDoctors(SearchDoctorDto dto)
        {
            IEnumerable<BookingClinic.Data.Entities.Doctor>? doctors = null;
            try
            {
                doctors = _userRepository.GetSearchDoctors();

                if (!string.IsNullOrEmpty(dto.Speciality))
                {
                    var speciality = _specialityRepository.GetSpecialityByName(dto.Speciality);

                    doctors = doctors.Where(d => d.SpecialityId == speciality.Id);
                }

                if (!string.IsNullOrEmpty(dto.Clinic))
                {
                    var clinic = _clinicRepository.GetClinicByName(dto.Clinic);

                    doctors = doctors.Where(d => d.ClinicId == clinic.Id);
                }
            }
            catch (Exception)
            {
                await _alertQueue.EnqueueAsync(
                     new AdminAlert("Db error", "Unable to retrieve data from db"));

                return ServiceResult<IEnumerable<SearchDoctorResDto>>.Failure(
                    new List<ServiceError>() { ServiceError.UnexpectedError() });
            }

            if (!string.IsNullOrEmpty(dto.Query))
            {
                var nameSurname = dto.Query.Trim().Split(' ').Take(2).Select(s => s.ToLower()).ToArray();

                if (nameSurname.Count() == 2)
                {
                    doctors = doctors.Where(d =>
                    {
                        var name = d.Name.ToLower();
                        var surname = d.Surname.ToLower();

                        return name.Contains(nameSurname[0]) || name.Contains(nameSurname[1]) ||
                        surname.Contains(nameSurname[0]) || surname.Contains(nameSurname[1]);
                    });
                }
                else
                {
                    doctors = doctors.Where(d =>
                    {
                        var name = d.Name.ToLower();
                        var surname = d.Surname.ToLower();

                        return name.Contains(nameSurname[0]) || surname.Contains(nameSurname[0]);
                    });
                }
            }
            var res = doctors.ToList().Adapt<IEnumerable<SearchDoctorResDto>>();

            return ServiceResult<IEnumerable<SearchDoctorResDto>>.Success(res);
        }

        public ServiceResult<ClaimsPrincipal> LoginUser(LoginUserDto dto)
        {
            UserBase? user;
            try
            {
                user = _userRepository.GetUserByEmail(dto.Email);
            }
            catch (Exception)
            {
                return ServiceResult<ClaimsPrincipal>.Failure(
                    new List<ServiceError>() { ServiceError.UnexpectedError() });
            }

            if (user == null)
            {
                return ServiceResult<ClaimsPrincipal>.Failure(
                    new List<ServiceError>() { ServiceError.InvalidCredentials() });
            }

            var hash = GetPasswordHash(user, dto.Password);

            if (user.PasswordHash != hash)
            {
                return ServiceResult<ClaimsPrincipal>.Failure(
                    new List<ServiceError>() { ServiceError.InvalidCredentials() });
            }

            var claimsPrincipal = CreateClaimsPrincipal(user);

            return ServiceResult<ClaimsPrincipal>.Success(claimsPrincipal);
        }

        public async Task<ServiceResult<ClaimsPrincipal>> RegisterUser(RegisterUserDto dto)
        {
            UserBase? existingUser = _userRepository.GetUserByEmail(dto.Email);

            if (existingUser != null)
            {
                return ServiceResult<ClaimsPrincipal>.Failure(
                    new List<ServiceError>() { ServiceError.UserAlreadyExists() });
            }

            Patient newUser = new()
            {
                Id = Guid.NewGuid(),
                Name = dto.Name,
                Surname = dto.Surname,
                Email = dto.Email
            };

            var passwordHash = GetPasswordHash(newUser, dto.Password);
            newUser.PasswordHash = passwordHash;

            _userRepository.AddEntity(newUser);

            try
            {
                await _userRepository.SaveChangesAsync();
            }
            catch (Exception)
            {
                return ServiceResult<ClaimsPrincipal>.Failure(
                    new List<ServiceError>() { ServiceError.UnexpectedError() });
            }

            var claimsPrincipal = CreateClaimsPrincipal(newUser);

            return ServiceResult<ClaimsPrincipal>.Success(claimsPrincipal);
        }

        public ServiceResult<UserPageDataDto> GetUserData(ClaimsPrincipal userPrincipal)
        {
            var idClaim = userPrincipal.FindFirst(c => c.Type == ClaimTypes.NameIdentifier);

            if (idClaim == null)
            {
                return ServiceResult<UserPageDataDto>.Failure(
                    new List<ServiceError>() { ServiceError.Unauthorized() });
            }

            Guid id = Guid.Parse(idClaim.Value);

            var user = _userRepository.GetById(id);

            var result = user.Adapt<UserPageDataDto>();

            return ServiceResult<UserPageDataDto>.Success(result);
        }

        public async Task<ServiceResult<object>> UpdateUserPhoto(IFormFile file, ClaimsPrincipal principal)
        {
            var name = file.FileName;
            var idx = name.LastIndexOf('.');
            var newName = Guid.NewGuid().ToString() + name.Substring(idx);

            var userIdClaim = principal.FindFirst(c => c.Type == ClaimTypes.NameIdentifier);
            var userId = userIdClaim!.Value;

            var userEntity = _userRepository.GetById(Guid.Parse(userId));

            if (userEntity == null)
            {
                return ServiceResult<object>.Failure(
                    new List<ServiceError>() { ServiceError.Unauthorized() });
            }

            userEntity.ProfilePicture = newName;

            _userRepository.UpdateEntity(userEntity);

            try
            {
                var wwwrootPath = _hostEnvironment.WebRootPath;

                var path = Path.Combine(wwwrootPath, "profiles", "users", newName);

                using var newFile = File.Create(path);
                await file.CopyToAsync(newFile);

                await _userRepository.SaveChangesAsync();

                return ServiceResult<object>.Success(null);
            }
            catch (Exception)
            {
                return ServiceResult<object>.Failure(
                    new List<ServiceError>() { ServiceError.UnexpectedError() });
            }
        }

        public async Task<ServiceResult<UserPageDataDto>> UpdateUser(UserPageDataUpdateDto dto, ClaimsPrincipal principal)
        {
            var userIdClaim = principal.FindFirst(c => c.Type == ClaimTypes.NameIdentifier);
            var userId = userIdClaim!.Value;
            var user = _userRepository.GetById(Guid.Parse(userId));
            
            if (user == null)
            {
                return ServiceResult<UserPageDataDto>.Failure(
                    new List<ServiceError>() { ServiceError.Unauthorized() });
            }

            if (GetPasswordHash(user, dto.Password) != user.PasswordHash)
            {
                return ServiceResult<UserPageDataDto>.Failure(
                    new List<ServiceError>() { ServiceError.InvalidPassword() });
            }

            user.Name = dto.Name;
            user.Surname = dto.Surname;
            user.Email = dto.Email;
            user.Phone = dto.Phone;
            user.PasswordHash = GetPasswordHash(user, dto.Password);

            _userRepository.UpdateEntity(user);

            try
            {
                await _userRepository.SaveChangesAsync();
                return ServiceResult<UserPageDataDto>.Success(user.Adapt<UserPageDataDto>());
            }
            catch (DbUpdateException)
            {
                return ServiceResult<UserPageDataDto>.Failure(
                    new List<ServiceError>() { ServiceError.UserAlreadyExists() });
            }
            catch (Exception)
            {
                return ServiceResult<UserPageDataDto>.Failure(
                    new List<ServiceError>() { ServiceError.UnexpectedError() });
            }
        }
    }
}
