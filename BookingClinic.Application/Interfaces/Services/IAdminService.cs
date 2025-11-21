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
        Task<ServiceResult> UpdateUser(UserAdminDto user);
        Task<ServiceResult> DeleteUser(Guid id);
        Task<ServiceResult> RegisterUser(UserAdminDto dto);

        IEnumerable<ClinicAdminDto> GetAllClinics();
        Task<ServiceResult> CreateClinic(ClinicAdminDto clinic);
        Task<ServiceResult> UpdateClinic(ClinicAdminDto clinic);
        Task<ServiceResult> DeleteClinic(Guid id);

        IEnumerable<SpecialityAdminDto> GetAllSpecialities();
        Task<ServiceResult> CreateSpeciality(SpecialityAdminDto speciality);
        Task<ServiceResult> UpdateSpeciality(SpecialityAdminDto speciality);
        Task<ServiceResult> DeleteSpeciality(Guid id);
    }
}
