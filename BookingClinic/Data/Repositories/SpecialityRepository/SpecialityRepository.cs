using BookingClinic.Data.AppContext;
using BookingClinic.Data.Entities;
using BookingClinic.Data.Repositories.Abstraction;

namespace BookingClinic.Data.Repositories.SpecialityRepository
{
    public class SpecialityRepository : RepositoryBase<Speciality, Guid>, ISpecialityRepository
    {
        public SpecialityRepository(ApplicationContext context) 
            : base(context)
        {
        }
    }
}
