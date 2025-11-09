using System.Security.Claims;

namespace BookingClinic.Application.Interfaces.Helpers
{
    public interface IReviewsHelper
    {
        bool CanUserWriteReview(Guid doctorId, ClaimsPrincipal principal);
    }
}
