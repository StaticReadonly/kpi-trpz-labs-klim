using BookingClinic.Data.Entities;
using BookingClinic.Data.Repositories.Abstraction;

namespace BookingClinic.Data.Repositories.UserRepository
{
    public interface IUserRepository : IRepository<UserBase, Guid>
    {
        IEnumerable<Doctor> GetSearchDoctors();
        UserBase? GetUserByEmail(string email);
    }
}
