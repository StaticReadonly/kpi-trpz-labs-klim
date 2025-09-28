using BookingClinic.Services.Data.User;
using FluentValidation;

namespace BookingClinic.Services.Validators.User
{
    public class UserRegisterValidator : AbstractValidator<RegisterUserDto>
    {
        public UserRegisterValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required");

            RuleFor(x => x.Surname)
                .NotEmpty().WithMessage("Surname is required");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required")
                .EmailAddress().WithMessage("Invalid email");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required")
                .MinimumLength(10).WithMessage("Minimum password length is 10");
        }
    }
}
