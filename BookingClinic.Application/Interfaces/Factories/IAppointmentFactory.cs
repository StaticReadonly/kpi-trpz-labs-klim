using BookingClinic.Application.Data.Appointment;
using BookingClinic.Application.Data.Doctor;
using BookingClinic.Domain.Entities;

namespace BookingClinic.Application.Interfaces.Factories
{
    public interface IAppointmentFactory
    {
        Appointment CreateAppointment(MakeAppointmentDto dto, Clinic clinic, DateTime datetime, Guid userId);

        Appointment CreateAppointment(MakeAppointmentDocDto dto, Clinic clinic, DateTime datetime, Guid userId);
    }
}
