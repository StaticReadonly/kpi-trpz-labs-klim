using BookingClinic.Application.Data.User;
using FluentValidation;

namespace BookingClinic.Services.Validators.User
{
    public class UserLoginValidator : AbstractValidator<LoginUserDto>
    {
        public UserLoginValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required")
                .EmailAddress().WithMessage("Invalid email");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required")
                .MinimumLength(10).WithMessage("Minimum password length is 10");
        }
    }
}
