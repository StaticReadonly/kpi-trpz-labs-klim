using BookingClinic.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookingClinic.Infrastructure.EntityConfigs
{
    public class DoctorConfig : IEntityTypeConfiguration<Doctor>
    {
        void IEntityTypeConfiguration<Doctor>.Configure(EntityTypeBuilder<Doctor> builder)
        {
            builder.HasOne(d => d.Speciality)
                .WithMany(s => s.Doctors)
                .HasForeignKey(d => d.SpecialityId);
        }
    }
}
