using BookingClinic.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookingClinic.Data.Configs
{
    public class SpecialityConfig : IEntityTypeConfiguration<Speciality>
    {
        void IEntityTypeConfiguration<Speciality>.Configure(EntityTypeBuilder<Speciality> builder)
        {
            
        }
    }
}
