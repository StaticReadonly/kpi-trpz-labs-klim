using BookingClinic.Services.AppointmentObserver.Observer;

namespace BookingClinic.Services.AppointmentObserver.Subject
{
    public interface IAppointmentSubject
    {
        void Attach(IAppointmentObserver observer);
        void Detach(IAppointmentObserver observer);
        Task NotifyAsync(BookingClinic.Data.Entities.Appointment appointment, string email);
    }
}
