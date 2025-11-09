using BookingClinic.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookingClinic.Infrastructure.EntityConfigs
{
    public class SpecialityConfig : IEntityTypeConfiguration<Speciality>
    {
        void IEntityTypeConfiguration<Speciality>.Configure(EntityTypeBuilder<Speciality> builder)
        {
            
        }
    }
}
