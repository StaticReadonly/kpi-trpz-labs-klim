using BookingClinic.Domain.Entities;

namespace BookingClinic.Application.Interfaces.Helpers
{
    public interface IPasswordHelper
    {
        string GetPasswordHash(UserBase user, string incomingPassword);
        bool CheckPasswordEquality(UserBase user, string incomingPassword);
    }
}
