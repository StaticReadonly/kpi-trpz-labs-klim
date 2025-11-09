using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace BookingClinic.Data.AppContext
{
    public class ApplicationContext2 : DbContext
    {
        public ApplicationContext2(DbContextOptions<ApplicationContext2> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
