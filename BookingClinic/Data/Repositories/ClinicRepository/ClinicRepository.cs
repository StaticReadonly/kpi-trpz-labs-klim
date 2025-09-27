using BookingClinic.Data.AppContext;
using BookingClinic.Data.Entities;
using BookingClinic.Data.Repositories.Abstraction;

namespace BookingClinic.Data.Repositories.ClinicRepository
{
    public class ClinicRepository : RepositoryBase<Clinic, Guid>, IClinicRepository
    {
        public ClinicRepository(ApplicationContext context)
            : base(context)
        {

        }

        public Clinic? GetClinicByName(string name) =>
            _context.Set<Clinic>().FirstOrDefault(c => c.Name == name);
    }
}
