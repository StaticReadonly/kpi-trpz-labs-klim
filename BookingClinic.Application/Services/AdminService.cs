using BookingClinic.Application.Common;
using BookingClinic.Application.Data.Admin;
using BookingClinic.Application.Interfaces.Services;
using BookingClinic.Application.Interfaces.UnitOfWork;
using BookingClinic.Domain.Entities;
using Mapster;
using System.Security.Cryptography;
using System.Text;

namespace BookingClinic.Application.Services
{
    public class AdminService : IAdminService
    {
        private readonly IUnitOfWork _unitOfWork;

        public AdminService(
            IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }

        public IEnumerable<UserAdminDto> GetAllUsers()
        {
            var users = _unitOfWork.Users.GetAll();
            return users.Adapt<IEnumerable<UserAdminDto>>();
        }

        public async Task<ServiceResult<UserAdminDto>> GetUserById(Guid id)
        {
            try
            {
                var user = _unitOfWork.Users.GetById(id);
                if (user == null)
                    return ServiceResult<UserAdminDto>.Failure(new List<ServiceError> { ServiceError.UnexpectedError() });

                return ServiceResult<UserAdminDto>.Success(user.Adapt<UserAdminDto>());
            }
            catch (Exception)
            {
                return ServiceResult<UserAdminDto>.Failure(new List<ServiceError> { ServiceError.UnexpectedError() });
            }
        }

        public async Task<ServiceResult<object>> UpdateUser(UserAdminDto dto)
        {
            try
            {
                if (dto.Id == null || dto.Id == Guid.Empty)
                    return ServiceResult<object>.Failure(new List<ServiceError> { ServiceError.UnexpectedError() });

                
                var existing = _unitOfWork.Users.GetById(dto.Id.Value);
                if (existing == null)
                    return ServiceResult<object>.Failure(new List<ServiceError> { ServiceError.UnexpectedError() });

                existing.Name = dto.Name;
                existing.Surname = dto.Surname;
                existing.Email = dto.Email;
                existing.Phone = dto.Phone;

                if (existing.Email != dto.Email || existing.Phone != dto.Phone || !string.IsNullOrEmpty(dto.Password))
                {
                    if (string.IsNullOrEmpty(dto.Password))
                    {
                        return ServiceResult<object>.Failure(new List<ServiceError> { ServiceError.UnexpectedError() });
                    }

                    var newHash = GetPasswordHash(existing, dto.Password);
                    existing.PasswordHash = newHash;
                }

                if (existing is Doctor doc)
                {
                    if (dto.ClinicId.HasValue)
                        doc.ClinicId = dto.ClinicId.Value;

                    if (dto.SpecialityId.HasValue)
                        doc.SpecialityId = dto.SpecialityId.Value;
                }

                _unitOfWork.Users.UpdateEntity(existing);
                await _unitOfWork.SaveChangesAsync();

                return ServiceResult<object>.Success(null);
            }
            catch (Exception)
            {
                return ServiceResult<object>.Failure(new List<ServiceError> { ServiceError.UnexpectedError() });
            }
        }

        public async Task<ServiceResult<object>> DeleteUser(Guid id)
        {
            try
            {
                var user = _unitOfWork.Users.GetById(id);
                if (user == null)
                    return ServiceResult<object>.Failure(new List<ServiceError> { ServiceError.UnexpectedError() });

                _unitOfWork.Users.DeleteEntity(user);
                await _unitOfWork.SaveChangesAsync();

                return ServiceResult<object>.Success(null);
            }
            catch (Exception)
            {
                return ServiceResult<object>.Failure(new List<ServiceError> { ServiceError.UnexpectedError() });
            }
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

        public async Task<ServiceResult<object>> RegisterUser(UserAdminDto dto)
        {
            UserBase? existingUser = _unitOfWork.Users.GetUserByEmail(dto.Email);

            if (existingUser != null)
            {
                return ServiceResult<object>.Failure(
                    new List<ServiceError>() { ServiceError.UserAlreadyExists() });
            }

            UserBase newUser;

            if (string.Equals(dto.Role, "Doctor", StringComparison.OrdinalIgnoreCase))
            {
                newUser = new Doctor
                {
                    Id = Guid.NewGuid(),
                    Name = dto.Name,
                    Surname = dto.Surname,
                    Email = dto.Email,
                    Phone = dto.Phone,
                    Role = dto.Role,
                    ClinicId = dto.ClinicId ?? Guid.Empty,
                    SpecialityId = dto.SpecialityId ?? Guid.Empty
                };
            }
            else
            {
                newUser = new UserBase
                {
                    Id = Guid.NewGuid(),
                    Name = dto.Name,
                    Surname = dto.Surname,
                    Email = dto.Email,
                    Phone = dto.Phone,
                    Role = dto.Role
                };
            }

            var passwordHash = GetPasswordHash(newUser, dto.Password);
            newUser.PasswordHash = passwordHash;

            _unitOfWork.Users.AddEntity(newUser);

            try
            {
                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception)
            {
                return ServiceResult<object>.Failure(
                    new List<ServiceError>() { ServiceError.UnexpectedError() });
            }

            return ServiceResult<object>.Success(null);
        }

        public IEnumerable<ClinicAdminDto> GetAllClinics()
        {
            var clinics = _unitOfWork.Clinics.GetAll();
            return clinics.Adapt<IEnumerable<ClinicAdminDto>>();
        }

        public async Task<ServiceResult<object>> CreateClinic(ClinicAdminDto dto)
        {
            try
            {
                var clinic = new Clinic
                {
                    Id = Guid.NewGuid(),
                    Name = dto.Name,
                    City = dto.City,
                    Street = dto.Street,
                    Building = dto.Building,
                    CreatedDate = DateTime.UtcNow
                };

                _unitOfWork.Clinics.AddEntity(clinic);
                await _unitOfWork.SaveChangesAsync();

                return ServiceResult<object>.Success(null);
            }
            catch (Exception)
            {
                return ServiceResult<object>.Failure(new List<ServiceError> { ServiceError.UnexpectedError() });
            }
        }

        public async Task<ServiceResult<object>> UpdateClinic(ClinicAdminDto dto)
        {
            try
            {
                if (dto.Id == null || dto.Id == Guid.Empty)
                    return ServiceResult<object>.Failure(new List<ServiceError> { ServiceError.UnexpectedError() });

                var existing = _unitOfWork.Clinics.GetById(dto.Id.Value);
                if (existing == null)
                    return ServiceResult<object>.Failure(new List<ServiceError> { ServiceError.UnexpectedError() });

                existing.Name = dto.Name;
                existing.City = dto.City;
                existing.Street = dto.Street;
                existing.Building = dto.Building;

                _unitOfWork.Clinics.UpdateEntity(existing);
                await _unitOfWork.SaveChangesAsync();

                return ServiceResult<object>.Success(null);
            }
            catch (Exception)
            {
                return ServiceResult<object>.Failure(new List<ServiceError> { ServiceError.UnexpectedError() });
            }
        }

        public async Task<ServiceResult<object>> DeleteClinic(Guid id)
        {
            try
            {
                var clinic = _unitOfWork.Clinics.GetById(id);
                if (clinic == null)
                    return ServiceResult<object>.Failure(new List<ServiceError> { ServiceError.UnexpectedError() });

                _unitOfWork.Clinics.DeleteEntity(clinic);
                await _unitOfWork.SaveChangesAsync();
                return ServiceResult<object>.Success(null);
            }
            catch (Exception)
            {
                return ServiceResult<object>.Failure(new List<ServiceError> { ServiceError.UnexpectedError() });
            }
        }

        public IEnumerable<SpecialityAdminDto> GetAllSpecialities()
        {
            var specs = _unitOfWork.Specialities.GetAll();
            return specs.Adapt<IEnumerable<SpecialityAdminDto>>();
        }

        public async Task<ServiceResult<object>> CreateSpeciality(SpecialityAdminDto dto)
        {
            try
            {
                var s = new Speciality
                {
                    Id = Guid.NewGuid(),
                    Name = dto.Name
                };

                _unitOfWork.Specialities.AddEntity(s);
                await _unitOfWork.SaveChangesAsync();

                return ServiceResult<object>.Success(null);
            }
            catch (Exception)
            {
                return ServiceResult<object>.Failure(new List<ServiceError> { ServiceError.UnexpectedError() });
            }
        }

        public async Task<ServiceResult<object>> UpdateSpeciality(SpecialityAdminDto dto)
        {
            try
            {
                if (dto.Id == null || dto.Id == Guid.Empty)
                    return ServiceResult<object>.Failure(new List<ServiceError> { ServiceError.UnexpectedError() });

                var existing = _unitOfWork.Specialities.GetById(dto.Id.Value);
                if (existing == null)
                    return ServiceResult<object>.Failure(new List<ServiceError> { ServiceError.UnexpectedError() });

                existing.Name = dto.Name;
                _unitOfWork.Specialities.UpdateEntity(existing);
                await _unitOfWork.SaveChangesAsync();

                return ServiceResult<object>.Success(null);
            }
            catch (Exception)
            {
                return ServiceResult<object>.Failure(new List<ServiceError> { ServiceError.UnexpectedError() });
            }
        }

        public async Task<ServiceResult<object>> DeleteSpeciality(Guid id)
        {
            try
            {
                var s = _unitOfWork.Specialities.GetById(id);
                if (s == null)
                    return ServiceResult<object>.Failure(new List<ServiceError> { ServiceError.UnexpectedError() });

                _unitOfWork.Specialities.DeleteEntity(s);
                await _unitOfWork.SaveChangesAsync();

                return ServiceResult<object>.Success(null);
            }
            catch (Exception)
            {
                return ServiceResult<object>.Failure(new List<ServiceError> { ServiceError.UnexpectedError() });
            }
        }
    }
}
