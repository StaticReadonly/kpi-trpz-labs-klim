using BookingClinic.Data.Repositories.UserRepository;
using BookingClinic.Data.Repositories.AppointmentRepository;
using BookingClinic.Data.Repositories.ClinicRepository;
using BookingClinic.Data.Repositories.DoctorReviewRepository;
using BookingClinic.Data.Repositories.SpecialityRepository;
using BookingClinic.Services.Appointment;

namespace BookingClinic.Data.Extensions
{
    public static class AppRepositoriesExtensions
    {
        public static IServiceCollection AddAppRepositories2(this IServiceCollection services)
        {
            services.AddScoped<IAppointmentRepository, AppointmentRepository>();
            services.AddScoped<IClinicRepository, ClinicRepository>();
            services.AddScoped<IDoctorReviewRepository, DoctorReviewRepository>();
            services.AddScoped<ISpecialityRepository, SpecialityRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IAppointmentService, AppointmentService>();

            return services;
        }
    }
}
