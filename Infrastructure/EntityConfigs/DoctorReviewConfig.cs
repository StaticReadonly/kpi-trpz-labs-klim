using BookingClinic.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookingClinic.Infrastructure.EntityConfigs
{
    public class DoctorReviewConfig : IEntityTypeConfiguration<DoctorReview>
    {
        public void Configure(EntityTypeBuilder<DoctorReview> builder)
        {
            builder.HasOne(dr => dr.Doctor)
                .WithMany(d => d.DoctorReviews)
                .HasForeignKey(dr => dr.DoctorId);

            builder.HasOne(dr => dr.Patient)
                .WithMany(p => p.ClientReviews)
                .HasForeignKey(dr => dr.PatientId);
        }
    }
}
