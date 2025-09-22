using BookingClinic.Data.Entities;
using BookingClinic.Data.Repositories.Abstraction;

namespace BookingClinic.Data.Repositories.DoctorReviewRepository
{
    public interface IDoctorReviewRepository : IRepository<DoctorReview, Guid>
    {
    }
}
