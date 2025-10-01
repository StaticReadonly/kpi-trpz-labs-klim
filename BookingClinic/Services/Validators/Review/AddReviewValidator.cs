using BookingClinic.Services.Data.Review;
using FluentValidation;

namespace BookingClinic.Services.Validators.Review
{
    public class AddReviewValidator : AbstractValidator<AddReviewDto>
    {
        public AddReviewValidator()
        {
            RuleFor(r => r.DoctorId).NotEmpty();
            RuleFor(r => r.Text).NotEmpty();
            RuleFor(r => r.Rating).GreaterThanOrEqualTo(1).LessThanOrEqualTo(5);
        }
    }
}
