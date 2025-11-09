using BookingClinic.Infrastructure.AppContext;
using BookingClinic.Services.Appointment;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BookingClinic.Infrastructure.Extensions
{
    public static class AppDbContextExtensions
    {
        public static IServiceCollection AddAppDbContext(this IServiceCollection services, ConfigurationManager manager)
        {
            services.AddDbContext<ApplicationContext>(cfg =>
            {
                cfg.UseNpgsql(manager.GetConnectionString("Main"));
            });

            return services;
        }
    }
}
