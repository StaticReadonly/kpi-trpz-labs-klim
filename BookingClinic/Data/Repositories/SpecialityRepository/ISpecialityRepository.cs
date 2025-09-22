using BookingClinic.Data.Entities;
using BookingClinic.Data.Repositories.Abstraction;

namespace BookingClinic.Data.Repositories.SpecialityRepository
{
    public interface ISpecialityRepository : IRepository<Speciality, Guid>
    {
    }
}
