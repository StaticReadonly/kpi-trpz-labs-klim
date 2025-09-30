using System.Security.Claims;

namespace BookingClinic.Services.Helpers.ReviewsHelper
{
    public interface IReviewsHelper
    {
        bool CanUserWriteReview(Guid doctorId, ClaimsPrincipal principal);
    }
}
