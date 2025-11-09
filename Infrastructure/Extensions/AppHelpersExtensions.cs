using BookingClinic.Application.Helpers;
using BookingClinic.Application.Interfaces;
using BookingClinic.Application.Interfaces.Helpers;
using BookingClinic.Domain.Interfaces;
using BookingClinic.Domain.Services;
using BookingClinic.Infrastructure.Helpers;
using Microsoft.Extensions.DependencyInjection;

namespace BookingClinic.Infrastructure.Extensions
{
    public static class AppHelpersExtensions
    {
        public static IServiceCollection AddAppHelpers(this IServiceCollection services)
        {
            services.AddScoped(typeof(IPaginationHelper<>), typeof(PaginationHelper<>));
            services.AddScoped<IAppointmentDomainService, AppointmentDomainService>();
            services.AddScoped<IReviewsHelper, ReviewsHelper>();
            services.AddScoped<IDoctorSorter, DoctorSorter>();
            services.AddScoped<IFileStorage, UserFileStorage>();

            return services;
        }
    }
}
