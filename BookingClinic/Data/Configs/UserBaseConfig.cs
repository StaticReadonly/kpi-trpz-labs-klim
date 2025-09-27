using BookingClinic.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookingClinic.Data.Configs
{
    public class UserBaseConfig : IEntityTypeConfiguration<UserBase>
    {
        public void Configure(EntityTypeBuilder<UserBase> builder)
        {
            builder.HasDiscriminator(u => u.Role)
                .HasValue<Patient>("Patient")
                .HasValue<Doctor>("Doctor")
                .HasValue<Admin>("Admin");

            builder.HasIndex(u => u.Email).IsUnique();
            builder.HasIndex(u => u.Phone).IsUnique();
        }
    }
}
