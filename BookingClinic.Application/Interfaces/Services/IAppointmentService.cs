using BookingClinic.Application.Common;
using BookingClinic.Application.Data.Appointment;
using BookingClinic.Application.Data.Doctor;

namespace BookingClinic.Application.Interfaces.Services
{
    public interface IAppointmentService
    {
        Task<ServiceResult> CreateAppointment(MakeAppointmentDto dto);
        Task<ServiceResult> CreateAppointmentDoctor(MakeAppointmentDocDto dto);
        ServiceResult<List<PatientAppointmentDto>> GetPatientAppointments();
        ServiceResult<List<DoctorAppointmentDto>> GetDoctorAppointments();
        Task<ServiceResult> CancelAppointment(Guid appId);
        Task<ServiceResult> FinishAppointment(FinishAppointmentDto dto);
    }
}
