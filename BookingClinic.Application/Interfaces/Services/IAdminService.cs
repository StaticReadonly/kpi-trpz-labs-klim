using BookingClinic.Application.Common;
using BookingClinic.Application.Data.Admin;
using BookingClinic.Application.Data.User;
using System.Security.Claims;

namespace BookingClinic.Application.Interfaces.Services
{
    public interface IAdminService
    {
        IEnumerable<UserAdminDto> GetAllUsers();
        Task<ServiceResult<UserAdminDto>> GetUserById(Guid id);
        Task<ServiceResult<object>> UpdateUser(UserAdminDto user);
        Task<ServiceResult<object>> DeleteUser(Guid id);
        Task<ServiceResult<object>> RegisterUser(UserAdminDto dto);

        IEnumerable<ClinicAdminDto> GetAllClinics();
        Task<ServiceResult<object>> CreateClinic(ClinicAdminDto clinic);
        Task<ServiceResult<object>> UpdateClinic(ClinicAdminDto clinic);
        Task<ServiceResult<object>> DeleteClinic(Guid id);

        IEnumerable<SpecialityAdminDto> GetAllSpecialities();
        Task<ServiceResult<object>> CreateSpeciality(SpecialityAdminDto speciality);
        Task<ServiceResult<object>> UpdateSpeciality(SpecialityAdminDto speciality);
        Task<ServiceResult<object>> DeleteSpeciality(Guid id);
    }
}
