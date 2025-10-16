using BookingClinic.Services.Data.Doctor;

namespace BookingClinic.Services.Doctor
{
    public interface IDoctorService
    {
        Task<ServiceResult<DoctorDataDto>> GetDoctorData(Guid doctorId);
    }
}
