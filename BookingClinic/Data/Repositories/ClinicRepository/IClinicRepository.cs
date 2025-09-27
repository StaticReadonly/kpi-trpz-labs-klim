using BookingClinic.Data.Entities;
using BookingClinic.Data.Repositories.Abstraction;

namespace BookingClinic.Data.Repositories.ClinicRepository
{
    public interface IClinicRepository : IRepository<Clinic, Guid>
    {
        Clinic? GetClinicByName(string name);
    }
}
