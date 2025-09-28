using BookingClinic.Data.Entities;
using BookingClinic.Data.Repositories.ClinicRepository;
using BookingClinic.Data.Repositories.SpecialityRepository;
using BookingClinic.Data.Repositories.UserRepository;
using BookingClinic.Services.Data.Doctor;
using BookingClinic.Services.Data.User;
using Mapster;
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

        public UserService(
            IUserRepository userRepository,
            ISpecialityRepository specialityRepository,
            IClinicRepository clinicRepository)
        {
            _userRepository = userRepository;
            _specialityRepository = specialityRepository;
            _clinicRepository = clinicRepository;
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

        public ServiceResult<IEnumerable<SearchDoctorResDto>> SearchDoctors(SearchDoctorDto dto)
        {
            try
            {
                var doctors = _userRepository.GetSearchDoctors();

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
            catch(Exception)
            {
                return ServiceResult<IEnumerable<SearchDoctorResDto>>.Failure(
                    new List<ServiceError>() { ServiceError.UnexpectedError() });
            }
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

            var claims = new Claim[]
            {
                new(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new(ClaimTypes.Role, user.Role)
            };

            var identity = new ClaimsIdentity(claims, "Cookie");

            return ServiceResult<ClaimsPrincipal>.Success(new ClaimsPrincipal(identity));
        }
    }
}
