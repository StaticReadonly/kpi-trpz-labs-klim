namespace BookingClinic.Services.NotificationService.EmailNotificationSender
{
    public class EmailSenderAdapter : INotificationSender
    {
        private readonly EmailNotificationSender _sender;

        public EmailSenderAdapter(EmailNotificationSender sender)
        {
            _sender = sender;
        }

        public async Task Send(string to, string subject, string message)
        {
            await _sender.SendEmail(to, subject, message);
        }
    }
}
