using BookingClinic.Application.Common;
using BookingClinic.Application.Data.Appointment;
using BookingClinic.Application.Data.Doctor;
using System.Security.Claims;

namespace BookingClinic.Application.Interfaces.Services
{
    public interface IAppointmentService
    {
        Task<ServiceResult<object>> CreateAppointment(MakeAppointmentDto dto, ClaimsPrincipal principal);
        Task<ServiceResult<object>> CreateAppointmentDoctor(MakeAppointmentDocDto dto, ClaimsPrincipal principal);
        ServiceResult<List<PatientAppointmentDto>> GetPatientAppointments(ClaimsPrincipal principal);
        ServiceResult<List<DoctorAppointmentDto>> GetDoctorAppointments(ClaimsPrincipal principal);
        Task<ServiceResult<object>> CancelAppointment(Guid appId);
        Task<ServiceResult<object>> FinishAppointment(FinishAppointmentDto dto);
    }
}
