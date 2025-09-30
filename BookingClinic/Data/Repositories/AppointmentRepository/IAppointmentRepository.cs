using BookingClinic.Data.Entities;
using BookingClinic.Data.Repositories.Abstraction;

namespace BookingClinic.Data.Repositories.AppointmentRepository
{
    public interface IAppointmentRepository : IRepository<Appointment, Guid>
    {
        Appointment? GetByDateTime(DateTime dateTime);
    }
}
