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

        public async Task<ServiceResult> UpdateUser(UserAdminDto user)
        {
            try
            {
                if (user.Id == null || user.Id == Guid.Empty)
                    return ServiceResult.Failure(ServiceError.UserIdCantBeEmpty());

                
                var existing = _unitOfWork.Users.GetById(user.Id.Value);
                if (existing == null)
                    return ServiceResult.Failure(ServiceError.UserNotFound());

                existing.Name = user.Name;
                existing.Surname = user.Surname;
                existing.Email = user.Email;
                existing.Phone = user.Phone;

                if (existing.Email != user.Email || existing.Phone != user.Phone || !string.IsNullOrEmpty(user.Password))
                {
                    if (string.IsNullOrEmpty(user.Password))
                    {
                        return ServiceResult.Failure(ServiceError.UserMustProvidePassword());
                    }

                    var newHash = _passwordHelper.GetPasswordHash(existing, user.Password);
                    existing.PasswordHash = newHash;
                }

                if (existing is Doctor doc)
                {
                    if (user.ClinicId.HasValue)
                        doc.ClinicId = user.ClinicId.Value;

                    if (user.SpecialityId.HasValue)
                        doc.SpecialityId = user.SpecialityId.Value;
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

        public async Task<ServiceResult> CreateClinic(ClinicAdminDto clinic)
        {
            try
            {
                var newClinic = _clinicFactory.CreateClinic(clinic);
                
                _unitOfWork.Clinics.AddEntity(newClinic);
                await _unitOfWork.SaveChangesAsync();

                return ServiceResult.Success();
            }
            catch (Exception)
            {
                return ServiceResult.Failure(ServiceError.UnexpectedError());
            }
        }

        public async Task<ServiceResult> UpdateClinic(ClinicAdminDto clinic)
        {
            try
            {
                if (clinic.Id == null || clinic.Id == Guid.Empty)
                    return ServiceResult.Failure(ServiceError.ClinicIdCantBeEmpty());

                var existing = _unitOfWork.Clinics.GetById(clinic.Id.Value);
                if (existing == null)
                    return ServiceResult.Failure(ServiceError.ClinicNotFound());

                existing.Name = clinic.Name;
                existing.City = clinic.City;
                existing.Street = clinic.Street;
                existing.Building = clinic.Building;

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

        public async Task<ServiceResult> CreateSpeciality(SpecialityAdminDto speciality)
        {
            try
            {
                var s = _specialityFactory.CreateSpeciality(speciality);

                _unitOfWork.Specialities.AddEntity(s);
                await _unitOfWork.SaveChangesAsync();

                return ServiceResult.Success();
            }
            catch (Exception)
            {
                return ServiceResult.Failure(ServiceError.UnexpectedError());
            }
        }

        public async Task<ServiceResult> UpdateSpeciality(SpecialityAdminDto speciality)
        {
            try
            {
                if (speciality.Id == null || speciality.Id == Guid.Empty)
                    return ServiceResult.Failure(ServiceError.SpecialityIdCantBeEmpty());

                var existing = _unitOfWork.Specialities.GetById(speciality.Id.Value);
                if (existing == null)
                    return ServiceResult.Failure(ServiceError.SpecialityNotFound());

                existing.Name = speciality.Name;
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
