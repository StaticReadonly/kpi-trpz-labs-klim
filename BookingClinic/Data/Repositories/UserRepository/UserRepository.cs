using BookingClinic.Data.AppContext;
using BookingClinic.Data.Entities;
using BookingClinic.Data.Repositories.Abstraction;

namespace BookingClinic.Data.Repositories.UserRepository
{
    public class UserRepository : RepositoryBase<UserBase, Guid>, IUserRepository
    {
        public UserRepository(ApplicationContext context) 
            : base(context)
        {
        }
    }
}
