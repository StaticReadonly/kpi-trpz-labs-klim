using BookingClinic.Services.Data.Appointment;
using System.Security.Claims;

namespace BookingClinic.Services.Appointment
{
    public interface IAppointmentService
    {
        Task<ServiceResult<object>> CreateAppointment(MakeAppointmentDto dto, ClaimsPrincipal principal);
    }
}
