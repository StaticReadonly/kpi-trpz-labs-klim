using BookingClinic.Data.Entities;
using BookingClinic.Data.Repositories.Abstraction;

namespace BookingClinic.Data.Repositories.AppointmentRepository
{
    public interface IAppointmentRepository : IRepository<Appointment, Guid>
    {
        IEnumerable<Appointment> GetAllScheduledForDay(DateTime date);
        Appointment? GetByDateTime(DateTime dateTime);
        IEnumerable<Appointment> GetPatientDoctorAppointments(Guid patientId, Guid doctorId);
        IEnumerable<Appointment> GetPatientAppointments(Guid patientId);
        IEnumerable<Appointment> GetDoctorAppointments(Guid doctorId, DateTime currentDate);
    }
}
