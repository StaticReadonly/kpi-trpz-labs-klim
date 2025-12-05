using BookingClinic.Application.Data.Admin;
using BookingClinic.Domain.Entities;

namespace BookingClinic.Application.Interfaces.Factories
{
    public interface IClinicFactory
    {
        Clinic CreateClinic(ClinicAdminDto dto);
    }
}
