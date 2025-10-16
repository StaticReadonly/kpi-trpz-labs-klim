using Microsoft.Extensions.Options;
using Telegram.Bot;

namespace BookingClinic.Services.NotificationService.TelegramNotificationSender
{
    public class TelegramSenderAdapter : INotificationSender
    {
        private readonly TelegramBotClient _sender;
        private readonly TelegramNotificationSenderOptions _options;

        public TelegramSenderAdapter(
            IOptions<TelegramNotificationSenderOptions> options)
        {
            _options = options.Value;

            _sender = new TelegramBotClient(_options.ApiKey);
        }

        public async Task Send(string to, string subject, string message)
        {
            await _sender.SendMessage(_options.ChatId, message);
        }
    }
}
