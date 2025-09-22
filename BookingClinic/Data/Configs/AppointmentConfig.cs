using BookingClinic.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookingClinic.Data.Configs
{
    public class AppointmentConfig : IEntityTypeConfiguration<Appointment>
    {
        void IEntityTypeConfiguration<Appointment>.Configure(EntityTypeBuilder<Appointment> builder)
        {
            builder.HasOne(a => a.Doctor)
                .WithMany(d => d.DoctorAppointments)
                .HasForeignKey(a => a.DoctorId);

            builder.HasOne(a => a.Patient)
                .WithMany(p => p.ClientAppointments)
                .HasForeignKey(a => a.PatientId);
        }
    }
}
