namespace BookingClinic.Services.AppointmentObserver.Observer
{
    public interface IAppointmentObserver
    {
        Task UpdateAsync(BookingClinic.Data.Entities.Appointment appointment, string email);
    }
}
