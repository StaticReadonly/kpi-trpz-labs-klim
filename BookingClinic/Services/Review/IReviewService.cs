using BookingClinic.Services.Data.Review;
using System.Security.Claims;

namespace BookingClinic.Services.Review
{
    public interface IReviewService
    {
        ServiceResult<DoctorReviewsDto> GetDoctorReviews(Guid doctorId);
        Task<ServiceResult<object>> CreateReview(AddReviewDto dto, ClaimsPrincipal principal);
    }
}
