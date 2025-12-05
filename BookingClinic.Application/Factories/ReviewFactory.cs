using BookingClinic.Application.Data.Review;
using BookingClinic.Application.Interfaces.Factories;
using BookingClinic.Domain.Entities;

namespace BookingClinic.Application.Factories
{
    public class ReviewFactory : IReviewFactory
    {
        public DoctorReview CreateReview(AddReviewDto dto, Guid userId)
        {
            ArgumentNullException.ThrowIfNull(dto);
            ArgumentNullException.ThrowIfNull(dto.Text);

            return new DoctorReview()
            {
                Id = Guid.NewGuid(),
                DoctorId = dto.DoctorId,
                PatientId = userId,
                Rating = dto.Rating,
                Text = dto.Text
            };
        }
    }
}
