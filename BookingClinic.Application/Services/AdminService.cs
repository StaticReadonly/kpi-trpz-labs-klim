using BookingClinic.Application.Common;
using BookingClinic.Application.Data.Admin;
using BookingClinic.Application.Data.User;
using BookingClinic.Application.Interfaces.Repositories;
using BookingClinic.Application.Interfaces.Services;
using BookingClinic.Domain.Entities;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace BookingClinic.Application.Services
{
    public class AdminService : IAdminService
    {
        private readonly IUserRepository _userRepository;
        private readonly IClinicRepository _clinicRepository;
        private readonly ISpecialityRepository _specialityRepository;

        public AdminService(
            IUserRepository userRepository,
            IClinicRepository clinicRepository,
            ISpecialityRepository specialityRepository)
        {
            _userRepository = userRepository;
            _clinicRepository = clinicRepository;
            _specialityRepository = specialityRepository;
        }

        public IEnumerable<UserAdminDto> GetAllUsers()
        {
            var users = _userRepository.GetAll();
            return users.Select(MapToUserAdminDto).ToList();
        }

        public async Task<ServiceResult<UserAdminDto>> GetUserById(Guid id)
        {
            try
            {
                var user = _userRepository.GetById(id);
                if (user == null)
                    return ServiceResult<UserAdminDto>.Failure(new List<ServiceError> { ServiceError.UnexpectedError() });

                return ServiceResult<UserAdminDto>.Success(MapToUserAdminDto(user));
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

                var existing = _userRepository.GetById(dto.Id.Value);
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

                _userRepository.UpdateEntity(existing);
                await _userRepository.SaveChangesAsync();

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
                var user = _userRepository.GetById(id);
                if (user == null)
                    return ServiceResult<object>.Failure(new List<ServiceError> { ServiceError.UnexpectedError() });

                _userRepository.DeleteEntity(user);
                await _userRepository.SaveChangesAsync();

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
            UserBase? existingUser = _userRepository.GetUserByEmail(dto.Email);

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

            _userRepository.AddEntity(newUser);

            try
            {
                await _userRepository.SaveChangesAsync();
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
            var clinics = _clinicRepository.GetAll();
            return clinics.Select(MapToClinicAdminDto).ToList();
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

                _clinicRepository.AddEntity(clinic);
                await _clinicRepository.SaveChangesAsync();

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

                var existing = _clinicRepository.GetById(dto.Id.Value);
                if (existing == null)
                    return ServiceResult<object>.Failure(new List<ServiceError> { ServiceError.UnexpectedError() });

                existing.Name = dto.Name;
                existing.City = dto.City;
                existing.Street = dto.Street;
                existing.Building = dto.Building;

                _clinicRepository.UpdateEntity(existing);
                await _clinicRepository.SaveChangesAsync();

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
                var clinic = _clinicRepository.GetById(id);
                if (clinic == null)
                    return ServiceResult<object>.Failure(new List<ServiceError> { ServiceError.UnexpectedError() });

                _clinicRepository.DeleteEntity(clinic);
                await _clinicRepository.SaveChangesAsync();
                return ServiceResult<object>.Success(null);
            }
            catch (Exception)
            {
                return ServiceResult<object>.Failure(new List<ServiceError> { ServiceError.UnexpectedError() });
            }
        }

        public IEnumerable<SpecialityAdminDto> GetAllSpecialities()
        {
            var specs = _specialityRepository.GetAll();
            return specs.Select(MapToSpecialityAdminDto).ToList();
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

                _specialityRepository.AddEntity(s);
                await _specialityRepository.SaveChangesAsync();

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

                var existing = _specialityRepository.GetById(dto.Id.Value);
                if (existing == null)
                    return ServiceResult<object>.Failure(new List<ServiceError> { ServiceError.UnexpectedError() });

                existing.Name = dto.Name;
                _specialityRepository.UpdateEntity(existing);
                await _specialityRepository.SaveChangesAsync();

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
                var s = _specialityRepository.GetById(id);
                if (s == null)
                    return ServiceResult<object>.Failure(new List<ServiceError> { ServiceError.UnexpectedError() });

                _specialityRepository.DeleteEntity(s);
                await _specialityRepository.SaveChangesAsync();

                return ServiceResult<object>.Success(null);
            }
            catch (Exception)
            {
                return ServiceResult<object>.Failure(new List<ServiceError> { ServiceError.UnexpectedError() });
            }
        }

        private static UserAdminDto MapToUserAdminDto(UserBase u)
        {
            var dto = new UserAdminDto
            {
                Id = u.Id,
                Name = u.Name,
                Surname = u.Surname,
                Email = u.Email,
                Phone = u.Phone,
                Role = u.Role
            };

            if (u is Doctor d)
            {
                dto.ClinicId = d.ClinicId;
                dto.SpecialityId = d.SpecialityId;
            }

            return dto;
        }

        private static ClinicAdminDto MapToClinicAdminDto(Clinic c)
        {
            return new ClinicAdminDto
            {
                Id = c.Id,
                Name = c.Name,
                City = c.City,
                Street = c.Street,
                Building = c.Building,
            };
        }

        private static SpecialityAdminDto MapToSpecialityAdminDto(Speciality s)
        {
            return new SpecialityAdminDto
            {
                Id = s.Id,
                Name = s.Name ?? string.Empty
            };
        }
    }
}
