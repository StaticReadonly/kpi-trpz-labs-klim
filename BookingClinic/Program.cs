using BookingClinic.Data.AppContext;
using BookingClinic.Data.Extensions;
using BookingClinic.Services.Appointment;
using BookingClinic.Services.Doctor.Facade;
using BookingClinic.Services.Extensions;
using BookingClinic.Services.Helpers.DoctorsSortingHelper.DoctorSorter;
using BookingClinic.Services.Helpers.DoctorsSortingHelper.DoctorSorterStrategies;
using BookingClinic.Services.Mapper;
using BookingClinic.Services.Options;
using BookingClinic.Services.Visitor;
using FluentValidation;
using Mapster;
using Microsoft.EntityFrameworkCore;
using QuestPDF.Infrastructure;
using System.Reflection;
using System.Text.Json.Serialization;

namespace BookingClinic
{
    public class Program
    {
        public static void Main(string[] args)
        {
            QuestPDF.Settings.License = LicenseType.Community;

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

            services.AddHostedService<AppointmentBackgroundService>();

            services.AddScoped<UserExportService>();
            services.AddScoped<IVisitorFactory, VisitorFactory>();

            services.AddScoped<ISearchDoctorFacade, SearchDoctorFacade>();
            services.AddScoped<IDoctorSorter, DoctorSorter>();

            services.Configure<DoctorSortingOptions>(cfg =>
            {
                cfg.Strategies = new Dictionary<string, IDoctorSorterStrategy>();
                var strats = cfg.Strategies;

                strats["Name Asc"] = new SortByNameAscStrategy();
                strats["Name Desc"] = new SortByNameDescStrategy();
            });

            services.AddMapster();
            services.AddAuthentication()
                .AddCookie("Cookie", cfg =>
                {
                    var cookieOpts = cfg.Cookie;
                    cookieOpts.HttpOnly = true;
                    cookieOpts.SameSite = SameSiteMode.Strict;
                    cookieOpts.MaxAge = TimeSpan.FromDays(14);
                    cookieOpts.Name = "Auth";

                    cfg.LoginPath = "/user/login";
                    cfg.LogoutPath = "/user/logout";
                });


            services.AddAuthorization(cfg =>
            {
                AuthorizationPolicies.BuildPatientOnlyPolicy(cfg);
                AuthorizationPolicies.BuildDoctorOnlyPolicy(cfg);
                AuthorizationPolicies.BuildAdminOnlyPolicy(cfg);
                AuthorizationPolicies.BuildUserOnlyPolicy(cfg);
            });

            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            MapperConfigs.RegisterMappings();

            var app = builder.Build();

            app.UseHttpsRedirection();

            app.UseExceptionHandler("/error/errPage");

            app.UseStaticFiles();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
