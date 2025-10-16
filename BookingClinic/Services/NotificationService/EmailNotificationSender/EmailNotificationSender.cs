using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;

namespace BookingClinic.Services.NotificationService.EmailNotificationSender
{
    public class EmailNotificationSender
    {
        private EmailNotificationSenderOptions _options;

        public EmailNotificationSender(IOptions<EmailNotificationSenderOptions> options)
        {
            _options = options.Value;
        }

        public async Task SendEmail(string email, string subject, string body)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("BookingClinic", _options.Username));
            message.To.Add(new MailboxAddress("", email));
            message.Subject = subject;
            message.Body = new TextPart("plain") 
            { 
                Text = body 
            };

            using SmtpClient client = new SmtpClient();
            await client.ConnectAsync(_options.Server, _options.Port, MailKit.Security.SecureSocketOptions.StartTls);
            await client.AuthenticateAsync(_options.Username, _options.Password);
            await client.SendAsync(message);
            await client.DisconnectAsync(true);
        }
    }
}
