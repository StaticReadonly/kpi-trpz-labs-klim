using BookingClinic.Services.Data.Doctor;

namespace BookingClinic.Services.UserService
{
    public interface IUserService
    {
        ServiceResult<IEnumerable<SearchDoctorResDto>> SearchDoctors(SearchDoctorDto dto);
    }
}
