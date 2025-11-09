using BookingClinic.Application.Common;

namespace BookingClinic.Application.Interfaces.Services
{
    public interface IClinicService
    {
        ServiceResult<IEnumerable<string>> GetClinicNames(); 
    }
}
