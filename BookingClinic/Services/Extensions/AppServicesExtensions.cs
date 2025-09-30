using BookingClinic.Services.Clinic;
using BookingClinic.Services.Doctor;
using BookingClinic.Services.Helpers.AppointmentHelper;
using BookingClinic.Services.Helpers.PaginationHelper;
using BookingClinic.Services.Helpers.ReviewsHelper;
using BookingClinic.Services.Review;
using BookingClinic.Services.Speciality;
using BookingClinic.Services.UserService;

namespace BookingClinic.Services.Extensions
{
    public static class AppServicesExtensions
    {
        public static IServiceCollection AddAppServices(this IServiceCollection services)
        {
            services.AddScoped(typeof(IPaginationHelper<>), typeof(PaginationHelper<>));
            services.AddScoped<IAppointmentHelper, AppointmentHelper>();
            services.AddScoped<IReviewsHelper, ReviewsHelper>();

            services.AddScoped<IUserService, UserService.UserService>();
            services.AddScoped<IClinicService, ClinicService>();
            services.AddScoped<ISpecialityService, SpecialityService>();
            services.AddScoped<IDoctorService, DoctorService>();
            services.AddScoped<IReviewService, ReviewService>();

            return services;
        }
    }
}
