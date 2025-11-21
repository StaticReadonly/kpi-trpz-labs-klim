using BookingClinic.Application.Common;
using BookingClinic.Application.Data.Review;

namespace BookingClinic.Application.Interfaces.Services
{
    public interface IReviewService
    {
        ServiceResult<DoctorReviewsDto> GetDoctorReviews(Guid doctorId);
        Task<ServiceResult> CreateReview(AddReviewDto dto);
    }
}
