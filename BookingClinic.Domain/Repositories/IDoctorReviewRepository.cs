using BookingClinic.Domain.Entities;

namespace BookingClinic.Domain.Interfaces.Repositories
{
    public interface IDoctorReviewRepository : IRepository<DoctorReview, Guid>
    {
        IEnumerable<DoctorReview> GetDoctorsReviews(Guid doctor);
        IEnumerable<DoctorReview> GetDoctorPatientReviews(Guid doctorId, Guid patientId);
    }
}
