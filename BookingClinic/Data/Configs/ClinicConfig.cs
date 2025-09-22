using BookingClinic.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookingClinic.Data.Configs
{
    public class ClinicConfig : IEntityTypeConfiguration<Clinic>
    {
        void IEntityTypeConfiguration<Clinic>.Configure(EntityTypeBuilder<Clinic> builder)
        {
        }
    }
}
