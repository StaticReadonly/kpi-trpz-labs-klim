using BookingClinic.Application.Common;
using BookingClinic.Application.Data.Doctor;

namespace BookingClinic.Application.Interfaces.Services
{
    public interface IDoctorService
    {
        ServiceResult<DoctorDataDto> GetDoctorData(Guid doctorId);
    }
}
