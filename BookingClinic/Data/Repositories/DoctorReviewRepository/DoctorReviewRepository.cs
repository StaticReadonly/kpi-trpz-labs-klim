using BookingClinic.Data.AppContext;
using BookingClinic.Data.Entities;
using BookingClinic.Data.Repositories.Abstraction;

namespace BookingClinic.Data.Repositories.DoctorReviewRepository
{
    public class DoctorReviewRepository : RepositoryBase<DoctorReview, Guid>, IDoctorReviewRepository
    {
        public DoctorReviewRepository(ApplicationContext context) 
            : base(context)
        {
        }
    }
}
