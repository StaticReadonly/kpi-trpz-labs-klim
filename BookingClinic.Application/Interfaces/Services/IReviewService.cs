using BookingClinic.Application.Common;
using BookingClinic.Application.Data.Review;
using System.Security.Claims;

namespace BookingClinic.Application.Interfaces.Services
{
    public interface IReviewService
    {
        ServiceResult<DoctorReviewsDto> GetDoctorReviews(Guid doctorId);
        Task<ServiceResult<object>> CreateReview(AddReviewDto dto, ClaimsPrincipal principal);
    }
}
