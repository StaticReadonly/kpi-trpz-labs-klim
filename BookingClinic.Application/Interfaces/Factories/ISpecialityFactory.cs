using BookingClinic.Application.Data.Admin;
using BookingClinic.Domain.Entities;

namespace BookingClinic.Application.Interfaces.Factories
{
    public interface ISpecialityFactory
    {
        Speciality CreateSpeciality(SpecialityAdminDto dto);
    }
}
