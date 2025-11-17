using BookingClinic.Application.Interfaces.UnitOfWork;
using BookingClinic.Infrastructure.UnitOfWork;
using Microsoft.Extensions.DependencyInjection;

namespace BookingClinic.Infrastructure.Extensions
{
    public static class AppUnitOfWorkExtensions
    {
        public static IServiceCollection AddAppUnitOfWork(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, AppUnitOfWork>();

            return services;
        }
    }
}
