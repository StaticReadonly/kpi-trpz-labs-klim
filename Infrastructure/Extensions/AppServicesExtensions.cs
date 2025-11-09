using BookingClinic.Application.Interfaces.Services;
using BookingClinic.Application.Interfaces.Visitor;
using BookingClinic.Application.Services;
using BookingClinic.Infrastructure.Services;
using BookingClinic.Services.Mapper;
using FluentValidation;
using Mapster;
using Microsoft.Extensions.DependencyInjection;
using QuestPDF.Infrastructure;
using System.Reflection;

namespace BookingClinic.Infrastructure.Extensions
{
    public static class AppServicesExtensions
    {
        public static IServiceCollection AddAppServices(this IServiceCollection services)
        {
            QuestPDF.Settings.License = LicenseType.Community;

            services.AddMapster();
            services.AddValidatorsFromAssembly(typeof(MapperConfigs).Assembly);
            MapperConfigs.RegisterMappings();

            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IClinicService, ClinicService>();
            services.AddScoped<ISpecialityService, SpecialityService>();
            services.AddScoped<IDoctorService, DoctorService>();
            services.AddScoped<IReviewService, ReviewService>();
            services.AddScoped<IAppointmentService, AppointmentService>();

            services.AddScoped<ISearchDoctorFacade, SearchDoctorFacade>();
            services.AddScoped<IVisitorFactory, VisitorFactory>();
            services.AddScoped<IUserExportService, UserExportService>();

            return services;
        }
    }
}
