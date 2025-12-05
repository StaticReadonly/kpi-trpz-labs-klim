using BookingClinic.Application.Data.Appointment;
using BookingClinic.Application.Data.Doctor;
using BookingClinic.Application.Interfaces.Factories;
using BookingClinic.Domain.Entities;

namespace BookingClinic.Application.Factories
{
    public class AppointmentFactory : IAppointmentFactory
    {
        public Appointment CreateAppointment(MakeAppointmentDto dto, Clinic clinic, DateTime datetime, Guid userId)
        {
            ArgumentNullException.ThrowIfNull(dto);
            ArgumentNullException.ThrowIfNull(clinic);

            return new Appointment()
            {
                Id = Guid.NewGuid(),
                PatientId = userId,
                DoctorId = dto.DoctorId,
                CreatedAt = DateTime.UtcNow,
                DateTime = datetime,
                Address = $"{clinic.Name}, {clinic.City} {clinic.Street} {clinic.Building}"
            };
        }

        public Appointment CreateAppointment(MakeAppointmentDocDto dto, Clinic clinic, DateTime datetime, Guid userId)
        {
            ArgumentNullException.ThrowIfNull(dto);
            ArgumentNullException.ThrowIfNull(clinic);

            return new Appointment()
            {
                Id = Guid.NewGuid(),
                PatientId = dto.PatientId,
                DoctorId = userId,
                CreatedAt = DateTime.UtcNow,
                DateTime = datetime,
                Address = $"{clinic.Name}, {clinic.City} {clinic.Street} {clinic.Building}"
            };
        }
    }
}
