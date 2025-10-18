using BookingClinic.Services.NotificationService.TelegramNotificationSender;
using Microsoft.Extensions.Options;
using Telegram.Bot;

namespace BookingClinic.Services.NotificationService
{
    public class NotificationSenderAdapter : INotificationSender
    {
        private readonly IServiceProvider _serviceProvider;

        public NotificationSenderAdapter(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task Send(string to, string subject, string message)
        {
            if (to.Contains("@"))
            {
                var emailService = _serviceProvider.GetRequiredService<EmailNotificationSender.EmailNotificationSender>();

                await emailService.SendEmail(to, subject, message);
            }
            else if (long.TryParse(to, out var chatId))
            {
                var telegramOptions = _serviceProvider.GetRequiredService<IOptions<TelegramNotificationSenderOptions>>();
                var telegramClient = new TelegramBotClient(telegramOptions.Value.ApiKey);
                await telegramClient.SendMessage(chatId, $"{subject}:\n{message}");
            }
            else
                throw new ArgumentException("Invalid type of address");
        }
    }
}
