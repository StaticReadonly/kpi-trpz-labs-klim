using BookingClinic.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookingClinic.Data.Configs
{
    public class PatientConfig : IEntityTypeConfiguration<Patient>
    {
        void IEntityTypeConfiguration<Patient>.Configure(EntityTypeBuilder<Patient> builder)
        {
            
        }
    }
}
