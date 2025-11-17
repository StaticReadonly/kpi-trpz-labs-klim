using BookingClinic.Application.Common;
using BookingClinic.Application.Data.Appointment;
using BookingClinic.Application.Data.Doctor;
using System.Security.Claims;

namespace BookingClinic.Application.Interfaces.Services
{
    public interface IAppointmentService
    {
        Task<ServiceResult<object>> CreateAppointment(MakeAppointmentDto dto);
        Task<ServiceResult<object>> CreateAppointmentDoctor(MakeAppointmentDocDto dto);
        ServiceResult<List<PatientAppointmentDto>> GetPatientAppointments();
        ServiceResult<List<DoctorAppointmentDto>> GetDoctorAppointments();
        Task<ServiceResult<object>> CancelAppointment(Guid appId);
        Task<ServiceResult<object>> FinishAppointment(FinishAppointmentDto dto);
    }
}
