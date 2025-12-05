using BookingClinic.Application.Data.Admin;
using BookingClinic.Application.Interfaces.Factories;
using BookingClinic.Domain.Entities;

namespace BookingClinic.Application.Factories
{
    public class ClinicFactory : IClinicFactory
    {
        public Clinic CreateClinic(ClinicAdminDto dto)
        {
            ArgumentNullException.ThrowIfNull(dto);
            ArgumentNullException.ThrowIfNull(dto.Name);
            ArgumentNullException.ThrowIfNull(dto.City);
            ArgumentNullException.ThrowIfNull(dto.Street);
            ArgumentNullException.ThrowIfNull(dto.Building);

            return new Clinic
            {
                Id = Guid.NewGuid(),
                Name = dto.Name,
                City = dto.City,
                Street = dto.Street,
                Building = dto.Building,
                CreatedDate = DateTime.UtcNow
            };
        }
    }
}
