using System.Threading.Channels;

namespace BookingClinic.Services.NotificationService.AdminNotificationService
{
    public class AdminAlertQueue : IAdminAlertQueue
    {
        private readonly Channel<AdminAlert> _channel;

        public AdminAlertQueue(int capacity = 100)
        {
            var options = new BoundedChannelOptions(capacity)
            {
                SingleReader = true,
                SingleWriter = false,
                FullMode = BoundedChannelFullMode.DropOldest
            };
            _channel = Channel.CreateBounded<AdminAlert>(options);
        }

        public ValueTask EnqueueAsync(AdminAlert alert, CancellationToken cancellationToken = default)
            => _channel.Writer.WriteAsync(alert, cancellationToken);

        public async IAsyncEnumerable<AdminAlert> ReadAllAsync([System.Runtime.CompilerServices.EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            while (await _channel.Reader.WaitToReadAsync(cancellationToken))
            {
                while (_channel.Reader.TryRead(out var item))
                    yield return item;
            }
        }
    }
}
