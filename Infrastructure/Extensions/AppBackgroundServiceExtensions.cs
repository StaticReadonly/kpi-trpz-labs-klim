using BookingClinic.Services.Appointment;
using Microsoft.Extensions.DependencyInjection;

namespace BookingClinic.Infrastructure.Extensions
{
    public static class AppBackgroundServiceExtensions
    {
        public static IServiceCollection AddAppBackgroundService(this IServiceCollection services)
        {
            services.AddHostedService<AppointmentBackgroundService>();

            return services;
        }
    }
}
