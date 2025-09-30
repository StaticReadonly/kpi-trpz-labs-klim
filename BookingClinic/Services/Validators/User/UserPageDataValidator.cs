using BookingClinic.Services.Data.User;
using FluentValidation;

namespace BookingClinic.Services.Validators.User
{
    public class UserPageDataValidator : AbstractValidator<UserPageDataDto>
    {
        public UserPageDataValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required");

            RuleFor(x => x.Surname)
                .NotEmpty().WithMessage("Surname is required");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required")
                .EmailAddress().WithMessage("Invalid email");

            RuleFor(x => x.Phone)
                .Matches("[0-9]{10}").When(x => !string.IsNullOrEmpty(x.Phone)).WithMessage("Invalid phone");
        }
    }
}
