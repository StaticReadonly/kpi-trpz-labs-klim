using Microsoft.Extensions.Options;

namespace BookingClinic.Services.NotificationService.AdminNotificationService
{
    public class AdminNotificationBg : BackgroundService
    {
        private readonly INotificationSender _notificationSender;
        private readonly AdminNotificationsOptions _options;
        private readonly ILogger<AdminNotificationBg> _logger;
        private readonly IAdminAlertQueue _alertQueue;

        public AdminNotificationBg(
            INotificationSender notificationSender,
            IOptions<AdminNotificationsOptions> options,
            ILogger<AdminNotificationBg> logger,
            IAdminAlertQueue alertQueue)
        {
            _notificationSender = notificationSender;
            _options = options.Value;
            _logger = logger;
            _alertQueue = alertQueue;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await foreach (var alert in _alertQueue.ReadAllAsync(stoppingToken))
                {
                    try
                    {
                        var emails = _options.Emails;
                        var chats = _options.ChatIds;

                        if (emails != null)
                        {
                            foreach (var email in emails)
                            {
                                await _notificationSender.Send(email, alert.Subject, alert.Message);
                            }
                        }

                        if (chats != null)
                        {
                            foreach (var chat in chats)
                            {
                                await _notificationSender.Send(chat.ToString(), alert.Subject, alert.Message);
                            }
                        }
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
