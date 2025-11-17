using BookingClinic.Application.Interfaces.UnitOfWork;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace BookingClinic.Services.Appointment
{
    public class AppointmentBackgroundService : BackgroundService
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly ILogger<AppointmentBackgroundService> _logger;

        public AppointmentBackgroundService(
            ILogger<AppointmentBackgroundService> logger,
            IServiceScopeFactory serviceScopeFactory)
        {
            _logger = logger;
            _serviceScopeFactory = serviceScopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using var timer = new PeriodicTimer(TimeSpan.FromMinutes(10));

            while (await timer.WaitForNextTickAsync(stoppingToken))
            {
                _logger.LogInformation("Tick");
                var now = DateTime.UtcNow;

                if (now.Hour < 22)
                {
                    continue;
                }

                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    IUnitOfWork unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

                    var appointments = unitOfWork.Appointments.GetUnfinishedAppointments(now).ToList();

                    if (appointments.Count == 0)
                    {
                        continue;
                    }

                    foreach (var a in appointments)
                    {
                        a.IsCanceled = true;
                        unitOfWork.Appointments.UpdateEntity(a);
                    }

                    try
                    {
                        if (appointments.Count != 0)
                        {
                            await unitOfWork.SaveChangesAsync();
                        }
                    }
                    catch (Exception exc)
                    {
                        _logger.LogError(exc, "Error while trying to cancel unfinished appointments");
                    }
                }
            }
        }
    }
}
