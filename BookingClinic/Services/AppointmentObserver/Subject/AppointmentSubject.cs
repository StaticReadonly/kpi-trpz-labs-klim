using BookingClinic.Services.AppointmentObserver.Observer;

namespace BookingClinic.Services.AppointmentObserver.Subject
{
    public class AppointmentSubject : IAppointmentSubject
    {
        private readonly object _sync = new();
        private readonly List<IAppointmentObserver> _observers;

        public AppointmentSubject()
        {
            _observers = new ();
        }

        public void Attach(IAppointmentObserver observer)
        {
            lock (_sync)
            {
                _observers.Add(observer);
            }
        }

        public void Detach(IAppointmentObserver observer)
        {
            lock (_sync)
            {
                _observers.Remove(observer);
            }
        }

        public async Task NotifyAsync(BookingClinic.Data.Entities.Appointment appointment, string email)
        {
            IAppointmentObserver[]? obs = null;
            lock (_sync)
            {
                obs = _observers.ToArray();
            }

            foreach (var o in obs)
            {
                await o.UpdateAsync(appointment, email);
            }
        }
    }
}
