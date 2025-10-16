
using Microsoft.Extensions.Options;

namespace BookingClinic.Services.NotificationService.AdminNotificationService
{
    public class AdminNotificationBg : BackgroundService
    {
        private readonly Func<string, INotificationSender> _notificationSenderFactory;
        private readonly AdminNotificationsOptions _options;
        private readonly ILogger<AdminNotificationBg> _logger;
        private readonly IAdminAlertQueue _alertQueue;

        public AdminNotificationBg(
            Func<string, INotificationSender> notificationSenderFactory,
            IOptions<AdminNotificationsOptions> options,
            ILogger<AdminNotificationBg> logger,
            IAdminAlertQueue alertQueue)
        {
            _notificationSenderFactory = notificationSenderFactory;
            _options = options.Value;
            _logger = logger;
            _alertQueue = alertQueue;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var emailSender = _notificationSenderFactory("email");
            var telegramSender = _notificationSenderFactory("telegram");

            while (!stoppingToken.IsCancellationRequested)
            {
                await foreach (var alert in _alertQueue.ReadAllAsync(stoppingToken))
                {
                    try
                    {
                        var recipients = _options.Emails;

                        if (recipients != null)
                        {
                            foreach (var email in recipients)
                            {
                                await emailSender.Send(email, alert.Subject, alert.Message);
                            }
                        }

                        await telegramSender.Send(string.Empty, string.Empty, alert.Message);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Failed to send admin alert");
                    }
                }
            }
        }
    }
}
