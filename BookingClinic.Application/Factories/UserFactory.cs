using BookingClinic.Application.Data.Admin;
using BookingClinic.Application.Data.User;
using BookingClinic.Application.Interfaces.Factories;
using BookingClinic.Application.Interfaces.Helpers;
using BookingClinic.Domain.Entities;

namespace BookingClinic.Application.Factories
{
    public class UserFactory : IUserFactory
    {
        private readonly IPasswordHelper _passwordHelper;

        public UserFactory(IPasswordHelper passwordHelper)
        {
            _passwordHelper = passwordHelper;
        }

        public UserBase CreateUser(UserAdminDto dto)
        {
            ArgumentNullException.ThrowIfNull(dto);
            ArgumentNullException.ThrowIfNull(dto.Name);
            ArgumentNullException.ThrowIfNull(dto.Surname);
            ArgumentNullException.ThrowIfNull(dto.Email);
            ArgumentNullException.ThrowIfNull(dto.Role);
            ArgumentNullException.ThrowIfNull(dto.Password);

            UserBase? res = null;
            if (string.Equals(dto.Role, "Doctor", StringComparison.OrdinalIgnoreCase))
            {
                res = new Doctor()
                {
                    Id = Guid.NewGuid(),
                    Name = dto.Name,
                    Surname = dto.Surname,
                    Email = dto.Email,
                    Phone = dto.Phone,
                    Role = dto.Role,
                    ClinicId = dto.ClinicId ?? Guid.Empty,
                    SpecialityId = dto.SpecialityId ?? Guid.Empty
                };
            }
            else
            {
                res = new UserBase()
                {
                    Id = Guid.NewGuid(),
                    Name = dto.Name,
                    Surname = dto.Surname,
                    Email = dto.Email,
                    Phone = dto.Phone,
                    Role = dto.Role
                };
            }

            var passwordHash = _passwordHelper.GetPasswordHash(res, dto.Password);
            res.PasswordHash = passwordHash;

            return res;
        }

        public UserBase CreateUser(RegisterUserDto dto)
        {
            ArgumentNullException.ThrowIfNull(dto);
            ArgumentNullException.ThrowIfNull(dto.Name);
            ArgumentNullException.ThrowIfNull(dto.Surname);
            ArgumentNullException.ThrowIfNull(dto.Email);

            var newUser =  new Patient()
            {
                Id = Guid.NewGuid(),
                Name = dto.Name,
                Surname = dto.Surname,
                Email = dto.Email
            };

            var passwordHash = _passwordHelper.GetPasswordHash(newUser, dto?.Password ?? string.Empty);
            newUser.PasswordHash = passwordHash;

            return newUser;
        }
    }
}
