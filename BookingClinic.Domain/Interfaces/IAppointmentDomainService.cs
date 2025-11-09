using BookingClinic.Domain.Entities;

namespace BookingClinic.Domain.Interfaces
{
    public interface IAppointmentDomainService
    {
        List<Tuple<string, IEnumerable<string>>> GetAppointments(Doctor doctor);
    }
}
