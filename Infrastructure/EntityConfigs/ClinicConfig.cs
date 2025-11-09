using BookingClinic.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookingClinic.Infrastructure.EntityConfigs
{
    public class ClinicConfig : IEntityTypeConfiguration<Clinic>
    {
        void IEntityTypeConfiguration<Clinic>.Configure(EntityTypeBuilder<Clinic> builder)
        {
        }
    }
}
