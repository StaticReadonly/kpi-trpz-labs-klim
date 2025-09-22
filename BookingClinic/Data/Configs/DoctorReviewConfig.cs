using BookingClinic.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookingClinic.Data.Configs
{
    public class DoctorReviewConfig : IEntityTypeConfiguration<DoctorReview>
    {
        void IEntityTypeConfiguration<DoctorReview>.Configure(EntityTypeBuilder<DoctorReview> builder)
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
