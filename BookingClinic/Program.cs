using BookingClinic.Data.AppContext;
using BookingClinic.Data.Extensions;
using BookingClinic.Services.Extensions;
using BookingClinic.Services.Helpers.DoctorsSortingHelper.DoctorSorter;
using BookingClinic.Services.Helpers.DoctorsSortingHelper.DoctorSorterStrategies;
using BookingClinic.Services.Mapper;
using BookingClinic.Services.NotificationService;
using BookingClinic.Services.NotificationService.AdminNotificationService;
using BookingClinic.Services.NotificationService.EmailNotificationSender;
using BookingClinic.Services.NotificationService.TelegramNotificationSender;
using BookingClinic.Services.Options;
using FluentValidation;
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

            services.AddScoped<IDoctorSorter, DoctorSorter>();
            services.AddSingleton<EmailNotificationSender>();

            builder.Services.AddSingleton<IAdminAlertQueue>(_ => new AdminAlertQueue(capacity: 500));
            services.AddHostedService<AdminNotificationBg>();

            services.AddTransient<INotificationSender, NotificationSenderAdapter>();

            services.Configure<AdminNotificationsOptions>(cfg =>
            {
                var section = config.GetSection("AdminNotifications");

                cfg.Emails = section.GetSection("Emails").Get<string[]>();
                cfg.ChatIds = section.GetSection("ChatIds").Get<long[]>();
            });
            services.Configure<TelegramNotificationSenderOptions>(cfg =>
            {
                var section = config.GetSection("Telegram");

                cfg.ApiKey = section["APIKey"];
            });
            services.Configure<EmailNotificationSenderOptions>(cfg =>
            {
                var section = config.GetSection("Gmail");

                cfg.Server = section["Server"];
                cfg.Port = int.Parse(section["Port"] ?? "0");
                cfg.Username = section["Username"];
                cfg.Password = section["Password"];
            });
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
                cfg.AddPolicy("AuthUser", cfg =>
                {
                    cfg.RequireAuthenticatedUser();
                });
                cfg.AddPolicy("Patients", cfg =>
                {
                    cfg.RequireAuthenticatedUser()
                        .RequireRole("Patient");
                });
                cfg.AddPolicy("Doctors", cfg =>
                {
                    cfg.RequireAuthenticatedUser()
                        .RequireRole("Doctor");
                });
                cfg.AddPolicy("Admin", cfg =>
                {
                    cfg.RequireAuthenticatedUser()
                        .RequireRole("Admin");
                });
                cfg.AddPolicy("PatientAppointment", cfg =>
                {
                    cfg.RequireAuthenticatedUser()
                        .RequireRole("Patient", "Admin")
                        .AddRequirements();
                });
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
