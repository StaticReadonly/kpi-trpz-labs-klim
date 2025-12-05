using BookingClinic.Application.Common;
using BookingClinic.Application.Data.Doctor;
using BookingClinic.Application.Data.User;
using BookingClinic.Application.Interfaces;
using BookingClinic.Application.Interfaces.Helpers;
using BookingClinic.Application.Interfaces.Services;
using BookingClinic.Application.Interfaces.UnitOfWork;
using BookingClinic.Domain.Entities;
using Mapster;
using System.Security.Claims;

namespace BookingClinic.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IFileStorage _fileStorage;
        private readonly IUserContextHelper _userContextHelper;
        private readonly IPasswordHelper _passwordHelper;

        public UserService(
            IUnitOfWork unitOfWork,
            IFileStorage fileStorage,
            IUserContextHelper userContextHelper,
            IPasswordHelper passwordHelper)
        {
            this._passwordHelper = passwordHelper;
            this._unitOfWork = unitOfWork;
            this._fileStorage = fileStorage;
            this._userContextHelper = userContextHelper;
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

        public ServiceResult<IEnumerable<SearchDoctorResDto>> SearchDoctors(SearchDoctorDto dto)
        {
            try
            {
                var doctors = _unitOfWork.Users.GetSearchDoctors();

                if (!string.IsNullOrEmpty(dto.Speciality))
                {
                    var speciality = _unitOfWork.Specialities.GetSpecialityByName(dto.Speciality);

                    if (speciality != null)
                    {
                        doctors = doctors.Where(d => d.SpecialityId == speciality.Id);
                    }
                }

                if (!string.IsNullOrEmpty(dto.Clinic))
                {
                    var clinic = _unitOfWork.Clinics.GetClinicByName(dto.Clinic);

                    if (clinic != null)
                    {
                        doctors = doctors.Where(d => d.ClinicId == clinic.Id);
                    }
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
                return ServiceResult<IEnumerable<SearchDoctorResDto>>.Failure(ServiceError.UnexpectedError());
            }
        }

        public ServiceResult<ClaimsPrincipal> LoginUser(LoginUserDto dto)
        {
            try
            {
                var user = _unitOfWork.Users.GetUserByEmail(dto?.Email ?? string.Empty);

                if (user == null)
                {
                    return ServiceResult<ClaimsPrincipal>.Failure(ServiceError.InvalidEmailOrPassword());
                }

                var check = _passwordHelper.CheckPasswordEquality(user, dto?.Password ?? string.Empty);

                if (!check)
                {
                    return ServiceResult<ClaimsPrincipal>.Failure(ServiceError.InvalidEmailOrPassword());
                }

                var claimsPrincipal = CreateClaimsPrincipal(user);

                return ServiceResult<ClaimsPrincipal>.Success(claimsPrincipal);
            }
            catch (Exception)
            {
                return ServiceResult<ClaimsPrincipal>.Failure(ServiceError.UnexpectedError());
            }
        }

        public async Task<ServiceResult<ClaimsPrincipal>> RegisterUser(RegisterUserDto dto)
        {
            try
            {
                UserBase? existingUser = _unitOfWork.Users.GetUserByEmail(dto?.Email ?? string.Empty);

                if (existingUser != null)
                {
                    return ServiceResult<ClaimsPrincipal>.Failure(ServiceError.UserAlreadyExists());
                }

                Patient newUser = new()
                {
                    Id = Guid.NewGuid(),
                    Name = dto.Name,
                    Surname = dto.Surname,
                    Email = dto.Email
                };

                var passwordHash = _passwordHelper.GetPasswordHash(newUser, dto?.Password ?? string.Empty);
                newUser.PasswordHash = passwordHash;

                _unitOfWork.Users.AddEntity(newUser);
                
                await _unitOfWork.SaveChangesAsync();
                var claimsPrincipal = CreateClaimsPrincipal(newUser);
                return ServiceResult<ClaimsPrincipal>.Success(claimsPrincipal);
            }
            catch (Exception)
            {
                return ServiceResult<ClaimsPrincipal>.Failure(ServiceError.UnexpectedError());
            }
        }

        public ServiceResult<UserPageDataDto> GetUserData()
        {
            try
            {
                var id = _userContextHelper.UserId!.Value;
                var user = _unitOfWork.Users.GetById(id);

                if (user == null)
                {
                    return ServiceResult<UserPageDataDto>.Failure(ServiceError.UserNotFound());
                }

                var result = user.Adapt<UserPageDataDto>();

                return ServiceResult<UserPageDataDto>.Success(result);
            }
            catch (Exception)
            {
                return ServiceResult<UserPageDataDto>.Failure(ServiceError.UnexpectedError());
            }
        }

        public async Task<ServiceResult> UpdateUserPhoto(UserPictureDto userPicture)
        {
            var id = _userContextHelper.UserId!.Value;

            var name = userPicture.FileName;
            var idx = name.LastIndexOf('.');
            var newName = Guid.NewGuid().ToString() + name.Substring(idx);
            userPicture.FileName = newName;

            var userEntity = _unitOfWork.Users.GetById(id);

            if (userEntity == null)
            {
                return ServiceResult.Failure(ServiceError.UserNotFound());
            }

            var transaction = await _unitOfWork.BeginTransactionAsync();

            userEntity.ProfilePicture = newName;
            _unitOfWork.Users.UpdateEntity(userEntity);

            try
            {
                await _unitOfWork.SaveChangesAsync();
                await _fileStorage.SaveUserPhotoAsync(userPicture, id);

                await transaction.CommitAsync();
                return ServiceResult.Success();
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                return ServiceResult.Failure(ServiceError.UnexpectedError());
            }
        }

        public async Task<ServiceResult<UserPageDataDto>> UpdateUser(UserPageDataUpdateDto dto)
        {
            var id = _userContextHelper.UserId!.Value;
            var user = _unitOfWork.Users.GetById(id);
            
            if (user == null)
            {
                return ServiceResult<UserPageDataDto>.Failure(ServiceError.UserNotFound());
            }

            var check = _passwordHelper.CheckPasswordEquality(user, dto.Password);

            if (!check)
            {
                return ServiceResult<UserPageDataDto>.Failure(ServiceError.InvalidEmailOrPassword());
            }

            user.Name = dto.Name;
            user.Surname = dto.Surname;
            user.Email = dto.Email;
            user.Phone = dto.Phone;
            user.PasswordHash = _passwordHelper.GetPasswordHash(user, dto.Password);

            _unitOfWork.Users.UpdateEntity(user);

            try
            {
                await _unitOfWork.SaveChangesAsync();
                return ServiceResult<UserPageDataDto>.Success(user.Adapt<UserPageDataDto>());
            }
            //catch (DbUpdateException)
            //{
            //    return ServiceResult<UserPageDataDto>.Failure(
            //        new List<ServiceError>() { ServiceError.UserAlreadyExists() });
            //}
            catch (Exception)
            {
                return ServiceResult<UserPageDataDto>.Failure(ServiceError.UnexpectedError());
            }
        }
    }
}
