using BookingClinic.Data.AppContext;
using BookingClinic.Data.Entities;
using BookingClinic.Data.Repositories.Abstraction;
using Microsoft.EntityFrameworkCore;

namespace BookingClinic.Data.Repositories.UserRepository
{
    public class UserRepository : RepositoryBase<UserBase, Guid>, IUserRepository
    {
        public UserRepository(ApplicationContext context) 
            : base(context)
        {
        }

        public IEnumerable<UserBase> GetVisitorAdmins() =>
            _dbSet.OfType<Admin>()
            .Include(a => a.ClientAppointments)
            .Include(a => a.ClientReviews)
            .ToList();

        public IEnumerable<UserBase> GetVisitorDoctors() =>
            _dbSet.OfType<Doctor>().Include(d => d.DoctorAppointments)
            .Include(d => d.DoctorReviews)
            .Include(d => d.Clinic)
            .Include(d => d.Speciality)
            .ToList();

        public IEnumerable<UserBase> GetVisitorPatients() =>
            _dbSet.OfType<Patient>().Include(p => p.ClientAppointments)
            .Include(p => p.ClientReviews)
            .ToList();

        public Doctor? GetDoctorById(Guid id) =>
            _context.Set<Doctor>().Include(d => d.Clinic).FirstOrDefault(d => d.Id == id);

        public Doctor? GetDoctorByIdWithAppClinicSpeciality(Guid id) =>
            _context.Set<Doctor>().Include(d => d.DoctorAppointments)
                                  .Include(d => d.Clinic)
                                  .Include(d => d.Speciality)
                                  .Include(d => d.DoctorReviews)
                                  .FirstOrDefault(d => d.Id == id);

        public IEnumerable<Doctor> GetSearchDoctors() => 
            _context.Set<Doctor>().Include(d => d.Speciality).Include(d => d.Clinic).ToList();

        public UserBase? GetUserByEmail(string email) =>
            _context.Set<UserBase>().FirstOrDefault(u => u.Email == email);
    }
}
