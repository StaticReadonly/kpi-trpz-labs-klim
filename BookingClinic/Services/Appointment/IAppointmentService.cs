using BookingClinic.Services.Data.Appointment;
using System.Security.Claims;

namespace BookingClinic.Services.Appointment
{
    public interface IAppointmentService
    {
        Task<ServiceResult<object>> CreateAppointment(MakeAppointmentDto dto, ClaimsPrincipal principal);
        ServiceResult<List<PatientAppointmentDto>> GetPatientAppointments(ClaimsPrincipal principal);
        ServiceResult<List<DoctorAppointmentDto>> GetDoctorAppointments(ClaimsPrincipal principal);
        Task<ServiceResult<object>> CancelAppointment(Guid appId);
        Task<ServiceResult<object>> FinishAppointment(FinishAppointmentDto dto);
    }
}
