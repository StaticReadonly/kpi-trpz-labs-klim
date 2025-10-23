using BookingClinic.Services.NotificationService;

namespace BookingClinic.Services.AppointmentObserver.Observer
{
    public class AppointmentObserver : IAppointmentObserver
    {
        private readonly INotificationSender _notificationSender;

        public AppointmentObserver(INotificationSender notificationSender)
        {
            _notificationSender = notificationSender;
        }

        public async Task UpdateAsync(BookingClinic.Data.Entities.Appointment appointment, string email)
        {
            string message = string.Empty;

            if (appointment.IsCanceled)
            {
                message = $"Hello! Your appointment scheduled for {appointment.DateTime} has been canceled! " +
                    $"You can check status in app.";
            }
            else if (appointment.IsFinished)
            {
                message = $"Hello! Your appointment scheduled for {appointment.DateTime} has been finished! " +
                    $"You can check results in app";
            }
            else
            {
                message = $"Hello! Your appointment scheduled for {appointment.DateTime} has been created! " +
                    $"You can check status in app";
            }
            
            await _notificationSender.Send(email, "Appointment status update", message);
        }
    }
}
