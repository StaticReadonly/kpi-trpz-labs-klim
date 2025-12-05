using BookingClinic.Application.Factories;
using BookingClinic.Application.Interfaces.Factories;
using Microsoft.Extensions.DependencyInjection;

namespace BookingClinic.Infrastructure.Extensions
{
    public static class AppFactoriesExtensions
    {
        public static IServiceCollection AddFactories(this IServiceCollection services)
        {
            services.AddScoped<IUserFactory, UserFactory>();
            services.AddScoped<IClinicFactory, ClinicFactory>();
            services.AddScoped<ISpecialityFactory, SpecialityFactory>();
            services.AddScoped<IAppointmentFactory, AppointmentFactory>();
            services.AddScoped<IReviewFactory, ReviewFactory>();

            return services;
        }
    }
}
