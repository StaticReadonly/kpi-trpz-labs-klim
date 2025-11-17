using BookingClinic.Infrastructure.Extensions;
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

            services.AddAppRepositories();
            services.AddAppServices();
            services.AddAppBackgroundService();
            services.AddAppDbContext(config);
            services.AddAppHelpers();
            services.AddAppOptions();
            services.AddAppUnitOfWork();

            services.AddHttpContextAccessor();
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
