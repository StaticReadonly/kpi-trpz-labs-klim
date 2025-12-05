using BookingClinic.Application.Data.Review;
using BookingClinic.Domain.Entities;

namespace BookingClinic.Application.Interfaces.Factories
{
    public interface IReviewFactory
    {
        DoctorReview CreateReview(AddReviewDto dto, Guid userId);
    }
}
