using BookingClinic.Services.Clinic;
using BookingClinic.Services.Speciality;
using BookingClinic.Services.UserService;

namespace BookingClinic.Services.Extensions
{
    public static class AppServicesExtensions
    {
        public static IServiceCollection AddAppServices(this IServiceCollection services)
        {
            services.AddScoped<IUserService, UserService.UserService>();
            services.AddScoped<IClinicService, ClinicService>();
            services.AddScoped<ISpecialityService, SpecialityService>();

            return services;
        }
    }
}
