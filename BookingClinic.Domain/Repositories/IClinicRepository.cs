using BookingClinic.Domain.Entities;

namespace BookingClinic.Domain.Interfaces.Repositories
{
    public interface IClinicRepository : IRepository<Clinic, Guid>
    {
        Clinic? GetClinicByName(string name);
    }
}
