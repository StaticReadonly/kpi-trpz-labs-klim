using BookingClinic.Application.Interfaces.Repositories;
using BookingClinic.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace BookingClinic.Infrastructure.Extensions
{
    public static class AppRepositoriesExtensions
    {
        public static IServiceCollection AddAppRepositories(this IServiceCollection services)
        {
            services.AddScoped<IAppointmentRepository, AppointmentRepository>();
            services.AddScoped<IClinicRepository, ClinicRepository>();
            services.AddScoped<IDoctorReviewRepository, DoctorReviewRepository>();
            services.AddScoped<ISpecialityRepository, SpecialityRepository>();
            services.AddScoped<IUserRepository, UserRepository>();

            return services;
        }
    }
}
