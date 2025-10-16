namespace BookingClinic.Services.NotificationService
{
    public interface INotificationSender
    {
        Task Send(string to, string subject, string message);
    }
}
