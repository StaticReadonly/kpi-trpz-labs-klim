namespace BookingClinic.Services.Helpers.AppointmentHelper
{
    public interface IAppointmentHelper
    {
        List<Tuple<string, IEnumerable<string>>> GetAppointments(BookingClinic.Data.Entities.Doctor doctor);
    }
}
