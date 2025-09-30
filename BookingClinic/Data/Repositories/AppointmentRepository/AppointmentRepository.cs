using BookingClinic.Data.AppContext;
using BookingClinic.Data.Entities;
using BookingClinic.Data.Repositories.Abstraction;

namespace BookingClinic.Data.Repositories.AppointmentRepository
{
    public class AppointmentRepository : RepositoryBase<Appointment, Guid>, IAppointmentRepository
    {
        public AppointmentRepository(ApplicationContext context) 
            : base(context)
        {
        }

        public Appointment? GetByDateTime(DateTime dateTime) =>
            _context.Set<Appointment>().FirstOrDefault(a => a.DateTime == dateTime);
    }
}
