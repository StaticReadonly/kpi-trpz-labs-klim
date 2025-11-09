using BookingClinic.Domain.Entities;

namespace BookingClinic.Application.Interfaces.Repositories
{
    public interface IClinicRepository : IRepository<Clinic, Guid>
    {
        Clinic? GetClinicByName(string name);
    }
}
