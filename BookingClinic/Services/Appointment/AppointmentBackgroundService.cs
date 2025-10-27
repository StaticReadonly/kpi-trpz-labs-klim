
using BookingClinic.Data.Repositories.AppointmentRepository;

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
            int attempts = 0;
            while (!stoppingToken.IsCancellationRequested)
            {
                var now = DateTime.UtcNow;

                if (now.Hour >= 22 && attempts < 5)
                {
                    attempts++;
                    using (var scope = _serviceScopeFactory.CreateScope())
                    {
                        IAppointmentRepository appRepo = scope.ServiceProvider.GetService<IAppointmentRepository>();

                        var appointments = appRepo.GetUnfinishedAppointments(now).ToList();

                        foreach (var a in appointments)
                        {
                            a.IsCanceled = true;
                            appRepo.UpdateEntity(a);
                        }

                        try
                        {
                            if (appointments.Count != 0)
                            {
                                await appRepo.SaveChangesAsync();
                            }
                        }
                        catch (Exception exc)
                        {
                            _logger.LogError(exc, "Error while trying to cancel unfinished appointments");
                        }
                    }
                }
                else if (now.Hour < 22)
                {
                    attempts = 0;
                }
                else
                {
                    await Task.Delay(TimeSpan.FromHours(1), stoppingToken);
                }
            }
        }
    }
}
