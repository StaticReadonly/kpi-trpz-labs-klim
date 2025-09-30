using BookingClinic.Services.Data.Review;

namespace BookingClinic.Services.Review
{
    public interface IReviewService
    {
        ServiceResult<DoctorReviewsDto> GetDoctorReviews(Guid doctorId);
    }
}
