using BookingClinic.Application.Interfaces.Repositories;
using BookingClinic.Domain.Entities;
using BookingClinic.Infrastructure.AppContext;
using Microsoft.EntityFrameworkCore;

namespace BookingClinic.Infrastructure.Repositories
{
    public class AppointmentRepository : RepositoryBase<Appointment, Guid>, IAppointmentRepository
    {
        public AppointmentRepository(ApplicationContext context) 
            : base(context)
        {
        }

        public Appointment? GetByDateTime(DateTime dateTime) =>
            _dbSet.FirstOrDefault(a => a.DateTime == dateTime);

        public IEnumerable<Appointment> GetDoctorAppointments(Guid doctorId, DateTime currentDate) =>
            _dbSet.Where(a => a.DoctorId == doctorId && a.DateTime.Date == currentDate && !a.IsFinished && !a.IsCanceled)
                    .Include(a => a.Patient);

        public IEnumerable<Appointment> GetPatientAppointments(Guid patientId) =>
            _dbSet.Where(a => a.PatientId == patientId)
                    .Include(a => a.Doctor).ThenInclude(d => d.Clinic);

        public IEnumerable<Appointment> GetPatientDoctorAppointments(Guid patientId, Guid doctorId) =>
            _dbSet.Where(a => a.DoctorId == doctorId && a.PatientId == patientId && a.IsFinished);

        public IEnumerable<Appointment> GetUnfinishedAppointments(DateTime currentDate) =>
            _dbSet.Where(a => a.DateTime.Date == currentDate.Date && !a.IsCanceled && !a.IsFinished);
    }
}
