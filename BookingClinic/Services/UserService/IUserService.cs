using BookingClinic.Services.Data.Doctor;
using BookingClinic.Services.Data.User;
using System.Security.Claims;

namespace BookingClinic.Services.UserService
{
    public interface IUserService
    {
        ServiceResult<IEnumerable<SearchDoctorResDto>> SearchDoctors(SearchDoctorDto dto);
        ServiceResult<ClaimsPrincipal> LoginUser(LoginUserDto dto);
    }
}
