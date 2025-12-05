using BookingClinic.Application.Common;
using BookingClinic.Application.Data.Admin;
using BookingClinic.Application.Interfaces.Factories;
using BookingClinic.Application.Interfaces.Helpers;
using BookingClinic.Application.Interfaces.Services;
using BookingClinic.Application.Interfaces.UnitOfWork;
using BookingClinic.Domain.Entities;
using Mapster;

namespace BookingClinic.Application.Services
{
    public class AdminService : IAdminService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPasswordHelper _passwordHelper;
        private readonly IUserFactory _userFactory;
        private readonly ISpecialityFactory _specialityFactory;
        private readonly IClinicFactory _clinicFactory;

        public AdminService(
            IUnitOfWork unitOfWork,
            IPasswordHelper passwordHelper,
            IUserFactory userFactory,
            ISpecialityFactory specialityFactory,
            IClinicFactory clinicFactory)
        {
            this._unitOfWork = unitOfWork;
            this._passwordHelper = passwordHelper;
            this._userFactory = userFactory;
            this._specialityFactory = specialityFactory;
            this._clinicFactory = clinicFactory;
        }

        public IEnumerable<UserAdminDto> GetAllUsers()
        {
            var users = _unitOfWork.Users.GetAll();
            return users.Adapt<IEnumerable<UserAdminDto>>();
        }

        public ServiceResult<UserAdminDto> GetUserById(Guid id)
        {
            try
            {
                var user = _unitOfWork.Users.GetById(id);
                if (user == null)
                    return ServiceResult<UserAdminDto>.Failure(ServiceError.UserNotFound());

                return ServiceResult<UserAdminDto>.Success(user.Adapt<UserAdminDto>());
            }
            catch (Exception)
            {
                return ServiceResult<UserAdminDto>.Failure(ServiceError.UnexpectedError());
            }
        }

        public async Task<ServiceResult> UpdateUser(UserAdminDto dto)
        {
            try
            {
                if (dto.Id == null || dto.Id == Guid.Empty)
                    return ServiceResult.Failure(ServiceError.UserIdCantBeEmpty());

                
                var existing = _unitOfWork.Users.GetById(dto.Id.Value);
                if (existing == null)
                    return ServiceResult.Failure(ServiceError.UserNotFound());

                existing.Name = dto.Name;
                existing.Surname = dto.Surname;
                existing.Email = dto.Email;
                existing.Phone = dto.Phone;

                if (existing.Email != dto.Email || existing.Phone != dto.Phone || !string.IsNullOrEmpty(dto.Password))
                {
                    if (string.IsNullOrEmpty(dto.Password))
                    {
                        return ServiceResult.Failure(ServiceError.UserMustProvidePassword());
                    }

                    var newHash = _passwordHelper.GetPasswordHash(existing, dto.Password);
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

                return ServiceResult.Success();
            }
            catch (Exception)
            {
                return ServiceResult.Failure(ServiceError.UnexpectedError());
            }
        }

        public async Task<ServiceResult> DeleteUser(Guid id)
        {
            try
            {
                var user = _unitOfWork.Users.GetById(id);
                if (user == null)
                    return ServiceResult.Failure(ServiceError.UserNotFound());

                _unitOfWork.Users.DeleteEntity(user);
                await _unitOfWork.SaveChangesAsync();

                return ServiceResult.Success();
            }
            catch (Exception)
            {
                return ServiceResult.Failure(ServiceError.UnexpectedError());
            }
        }

        public async Task<ServiceResult> RegisterUser(UserAdminDto dto)
        {
            UserBase? existingUser = _unitOfWork.Users.GetUserByEmail(dto.Email);

            if (existingUser != null)
            {
                return ServiceResult.Failure(ServiceError.UserAlreadyExists());
            }

            try
            {
                var newUser = _userFactory.CreateUser(dto);
                _unitOfWork.Users.AddEntity(newUser);

                await _unitOfWork.SaveChangesAsync();

                return ServiceResult.Success();
            }
            catch (Exception)
            {
                return ServiceResult.Failure(ServiceError.UnexpectedError());
            }
        }

        public IEnumerable<ClinicAdminDto> GetAllClinics()
        {
            var clinics = _unitOfWork.Clinics.GetAll();
            return clinics.Adapt<IEnumerable<ClinicAdminDto>>();
        }

        public async Task<ServiceResult> CreateClinic(ClinicAdminDto dto)
        {
            try
            {
                var clinic = _clinicFactory.CreateClinic(dto);
                
                _unitOfWork.Clinics.AddEntity(clinic);
                await _unitOfWork.SaveChangesAsync();

                return ServiceResult.Success();
            }
            catch (Exception)
            {
                return ServiceResult.Failure(ServiceError.UnexpectedError());
            }
        }

        public async Task<ServiceResult> UpdateClinic(ClinicAdminDto dto)
        {
            try
            {
                if (dto.Id == null || dto.Id == Guid.Empty)
                    return ServiceResult.Failure(ServiceError.ClinicIdCantBeEmpty());

                var existing = _unitOfWork.Clinics.GetById(dto.Id.Value);
                if (existing == null)
                    return ServiceResult.Failure(ServiceError.ClinicNotFound());

                existing.Name = dto.Name;
                existing.City = dto.City;
                existing.Street = dto.Street;
                existing.Building = dto.Building;

                _unitOfWork.Clinics.UpdateEntity(existing);
                await _unitOfWork.SaveChangesAsync();

                return ServiceResult.Success();
            }
            catch (Exception)
            {
                return ServiceResult.Failure(ServiceError.UnexpectedError());
            }
        }

        public async Task<ServiceResult> DeleteClinic(Guid id)
        {
            try
            {
                var clinic = _unitOfWork.Clinics.GetById(id);
                if (clinic == null)
                    return ServiceResult.Failure(ServiceError.ClinicNotFound());

                _unitOfWork.Clinics.DeleteEntity(clinic);
                await _unitOfWork.SaveChangesAsync();

                return ServiceResult.Success();
            }
            catch (Exception)
            {
                return ServiceResult.Failure(ServiceError.UnexpectedError());
            }
        }

        public IEnumerable<SpecialityAdminDto> GetAllSpecialities()
        {
            var specs = _unitOfWork.Specialities.GetAll();
            return specs.Adapt<IEnumerable<SpecialityAdminDto>>();
        }

        public async Task<ServiceResult> CreateSpeciality(SpecialityAdminDto dto)
        {
            try
            {
                var s = _specialityFactory.CreateSpeciality(dto);

                _unitOfWork.Specialities.AddEntity(s);
                await _unitOfWork.SaveChangesAsync();

                return ServiceResult.Success();
            }
            catch (Exception)
            {
                return ServiceResult.Failure(ServiceError.UnexpectedError());
            }
        }

        public async Task<ServiceResult> UpdateSpeciality(SpecialityAdminDto dto)
        {
            try
            {
                if (dto.Id == null || dto.Id == Guid.Empty)
                    return ServiceResult.Failure(ServiceError.SpecialityIdCantBeEmpty());

                var existing = _unitOfWork.Specialities.GetById(dto.Id.Value);
                if (existing == null)
                    return ServiceResult.Failure(ServiceError.SpecialityNotFound());

                existing.Name = dto.Name;
                _unitOfWork.Specialities.UpdateEntity(existing);
                await _unitOfWork.SaveChangesAsync();

                return ServiceResult.Success();
            }
            catch (Exception)
            {
                return ServiceResult.Failure(ServiceError.UnexpectedError());
            }
        }

        public async Task<ServiceResult> DeleteSpeciality(Guid id)
        {
            try
            {
                var s = _unitOfWork.Specialities.GetById(id);
                if (s == null)
                    return ServiceResult.Failure(ServiceError.SpecialityNotFound());

                _unitOfWork.Specialities.DeleteEntity(s);
                await _unitOfWork.SaveChangesAsync();

                return ServiceResult.Success();
            }
            catch (Exception)
            {
                return ServiceResult.Failure(ServiceError.UnexpectedError());
            }
        }
    }
}
