using BookingClinic.Application.Interfaces.Repositories;
using BookingClinic.Domain.Entities;
using BookingClinic.Infrastructure.AppContext;

namespace BookingClinic.Infrastructure.Repositories
{
    public class ClinicRepository : RepositoryBase<Clinic, Guid>, IClinicRepository
    {
        public ClinicRepository(ApplicationContext context)
            : base(context)
        {

        }

        public Clinic? GetClinicByName(string name) =>
            _dbSet.FirstOrDefault(c => c.Name == name);
    }
}
