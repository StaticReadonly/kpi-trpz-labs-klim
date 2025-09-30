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

        public Doctor? GetDoctorById(Guid id) =>
            _context.Set<Doctor>().Include(d => d.Clinic).FirstOrDefault(d => d.Id == id);

        public Doctor? GetDoctorByIdWithAppClinicSpeciality(Guid id) =>
            _context.Set<Doctor>().Include(d => d.DoctorAppointments)
                                  .Include(d => d.Clinic)
                                  .Include(d => d.Speciality)
                                  .Include(d => d.DoctorReviews)
                                  .FirstOrDefault();

        public IEnumerable<Doctor> GetSearchDoctors() => 
            _context.Set<Doctor>().Include(d => d.Speciality).Include(d => d.Clinic).ToList();

        public UserBase? GetUserByEmail(string email) =>
            _context.Set<UserBase>().FirstOrDefault(u => u.Email == email);
    }
}
