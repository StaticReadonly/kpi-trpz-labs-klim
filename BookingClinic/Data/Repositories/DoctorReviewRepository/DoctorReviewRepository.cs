using BookingClinic.Data.AppContext;
using BookingClinic.Data.Entities;
using BookingClinic.Data.Repositories.Abstraction;
using Microsoft.EntityFrameworkCore;

namespace BookingClinic.Data.Repositories.DoctorReviewRepository
{
    public class DoctorReviewRepository : RepositoryBase<DoctorReview, Guid>, IDoctorReviewRepository
    {
        public DoctorReviewRepository(ApplicationContext context) 
            : base(context)
        {
        }

        public IEnumerable<DoctorReview> GetDoctorPatientReviews(Guid doctorId, Guid patientId) =>
            _dbSet.Where(r => r.DoctorId == doctorId && r.PatientId == patientId);

        public IEnumerable<DoctorReview> GetDoctorsReviews(Guid doctor) =>
            _dbSet.Where(r => r.DoctorId == doctor).Include(r => r.Doctor).Include(r => r.Patient);
    }
}
