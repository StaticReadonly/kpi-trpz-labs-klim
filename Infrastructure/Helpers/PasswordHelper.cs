using BookingClinic.Application.Interfaces.Helpers;
using BookingClinic.Domain.Entities;
using System.Security.Cryptography;
using System.Text;

namespace BookingClinic.Infrastructure.Helpers
{
    public class PasswordHelper : IPasswordHelper
    {
        public bool CheckPasswordEquality(UserBase user, string incomingPassword)
        {
            string incomingHash = this.GetPasswordHash(user, incomingPassword);
            return string.Equals(incomingHash, user.PasswordHash, StringComparison.InvariantCulture);
        }

        public string GetPasswordHash(UserBase user, string incomingPassword)
        {
            using var sha256 = SHA256.Create();

            var salt = user.Id.ToString() + user.Email + user.Phone;
            var saltedPassword = $"{incomingPassword}:{salt}";
            var bytes = Encoding.UTF8.GetBytes(saltedPassword);
            var hash = sha256.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }
    }
}
