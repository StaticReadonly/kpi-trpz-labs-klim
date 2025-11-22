using BookingClinic.Domain.Entities;

namespace BookingClinic.Domain.Interfaces.Repositories
{
    public interface IAppointmentRepository : IRepository<Appointment, Guid>
    {
        Appointment? GetByDateTime(DateTime dateTime);
        IEnumerable<Appointment> GetPatientDoctorAppointments(Guid patientId, Guid doctorId);
        IEnumerable<Appointment> GetPatientAppointments(Guid patientId);
        IEnumerable<Appointment> GetDoctorAppointments(Guid doctorId, DateTime currentDate);
        IEnumerable<Appointment> GetUnfinishedAppointments(DateTime currentDate);
    }
}
