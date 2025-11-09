using BookingClinic.Application.Interfaces.Repositories;
using BookingClinic.Domain.Entities;
using BookingClinic.Infrastructure.AppContext;

namespace BookingClinic.Infrastructure.Repositories
{
    public class SpecialityRepository : RepositoryBase<Speciality, Guid>, ISpecialityRepository
    {
        public SpecialityRepository(ApplicationContext context) 
            : base(context)
        {
        }

        public Speciality? GetSpecialityByName(string name) => 
            _dbSet.FirstOrDefault(s => s.Name == name);
    }
}
