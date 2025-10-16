namespace BookingClinic.Services.NotificationService.AdminNotificationService
{
    public interface IAdminAlertQueue
    {
        ValueTask EnqueueAsync(AdminAlert alert, CancellationToken cancellationToken = default);
        IAsyncEnumerable<AdminAlert> ReadAllAsync(CancellationToken cancellationToken = default);
    }
}
