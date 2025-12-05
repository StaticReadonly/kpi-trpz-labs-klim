using BookingClinic.Application.Data.Admin;
using BookingClinic.Application.Data.User;
using BookingClinic.Domain.Entities;

namespace BookingClinic.Application.Interfaces.Factories
{
    public interface IUserFactory
    {
        UserBase CreateUser(UserAdminDto dto);

        UserBase CreateUser(RegisterUserDto dto);
    }
}
