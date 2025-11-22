using BookingClinic.Domain.Entities;

namespace BookingClinic.Domain.Interfaces.Repositories
{
    public interface ISpecialityRepository : IRepository<Speciality, Guid>
    {
        Speciality? GetSpecialityByName(string name);
    }
}
