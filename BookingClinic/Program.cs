using BookingClinic.Data.AppContext;
using BookingClinic.Data.Extensions;
using BookingClinic.Services.Extensions;
using BookingClinic.Services.Helpers.PaginationHelper;
using BookingClinic.Services.Mapper;
using Mapster;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using System.Text.Json.Serialization;

namespace BookingClinic
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var services = builder.Services;
            var config = builder.Configuration;

            config.AddUserSecrets(Assembly.GetExecutingAssembly());

            services.AddControllersWithViews()
                .AddJsonOptions(opts =>
                {
                    opts.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
                });

            services.AddDbContext<ApplicationContext>(cfg =>
            {
                cfg.UseNpgsql(config.GetConnectionString("Main"));
            });

            services.AddAppRepositories();
            services.AddAppServices();

            services.AddScoped(typeof(IPaginationHelper<>), typeof(PaginationHelper<>));

            services.AddMapster();
            services.AddAuthentication();
            services.AddAuthorization();

            MapperConfigs.RegisterMappings();

            var app = builder.Build();

            app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
