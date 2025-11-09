using BookingClinic.Data.AppContext;
using BookingClinic.Data.Entities;
using BookingClinic.Data.Repositories.Abstraction;

namespace BookingClinic.Data.Repositories.SpecialityRepository
{
    public class SpecialityRepository : RepositoryBase<Speciality, Guid>, ISpecialityRepository
    {
        public SpecialityRepository(ApplicationContext2 context) 
            : base(context)
        {
        }

        public Speciality? GetSpecialityByName(string name) => 
            _context.Set<Speciality>().FirstOrDefault(s => s.Name == name);
    }
}
