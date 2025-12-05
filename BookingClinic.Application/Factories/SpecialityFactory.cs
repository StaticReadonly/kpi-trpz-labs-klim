using BookingClinic.Application.Data.Admin;
using BookingClinic.Application.Interfaces.Factories;
using BookingClinic.Domain.Entities;

namespace BookingClinic.Application.Factories
{
    public class SpecialityFactory : ISpecialityFactory
    {
        public Speciality CreateSpeciality(SpecialityAdminDto dto)
        {
            ArgumentNullException.ThrowIfNull(dto);
            ArgumentNullException.ThrowIfNull(dto.Name);

            return new Speciality
            {
                Id = Guid.NewGuid(),
                Name = dto.Name
            };
        }
    }
}
