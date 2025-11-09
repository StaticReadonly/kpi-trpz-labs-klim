using BookingClinic.Domain.Entities;

namespace BookingClinic.Application.Interfaces.Repositories
{
    public interface ISpecialityRepository : IRepository<Speciality, Guid>
    {
        Speciality? GetSpecialityByName(string name);
    }
}
