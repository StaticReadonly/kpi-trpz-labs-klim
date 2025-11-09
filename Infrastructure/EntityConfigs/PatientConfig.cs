using BookingClinic.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookingClinic.Infrastructure.EntityConfigs
{
    public class PatientConfig : IEntityTypeConfiguration<Patient>
    {
        void IEntityTypeConfiguration<Patient>.Configure(EntityTypeBuilder<Patient> builder)
        {
            
        }
    }
}
