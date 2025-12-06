using BookingClinic.Application.Helpers;
using BookingClinic.Application.Helpers.DoctorSorterHelper;
using BookingClinic.Application.Options;
using Microsoft.Extensions.DependencyInjection;

namespace BookingClinic.Infrastructure.Extensions
{
    public static class AppOptionsExtensions
    {
        public static IServiceCollection AddAppOptions(this IServiceCollection services)
        {
            services.Configure<DoctorSortingOptions>(cfg =>
            {
                cfg.Strategies = new Dictionary<string, IDoctorSorterStrategy>();
                var strats = cfg.Strategies;

                strats["Name Asc"] = new SortByNameAscStrategy();
                strats["Name Desc"] = new SortByNameDescStrategy();
            });

            return services;
        }
    }
}
