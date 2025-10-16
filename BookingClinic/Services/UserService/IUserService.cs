using BookingClinic.Services.Data.Doctor;
using BookingClinic.Services.Data.User;
using System.Security.Claims;

namespace BookingClinic.Services.UserService
{
    public interface IUserService
    {
        Task<ServiceResult<IEnumerable<SearchDoctorResDto>>> SearchDoctors(SearchDoctorDto dto);
        ServiceResult<ClaimsPrincipal> LoginUser(LoginUserDto dto);
        Task<ServiceResult<ClaimsPrincipal>> RegisterUser(RegisterUserDto dto);
        ServiceResult<UserPageDataDto> GetUserData(ClaimsPrincipal userPrincipal);
        Task<ServiceResult<object>> UpdateUserPhoto(IFormFile file, ClaimsPrincipal principal);
        Task<ServiceResult<UserPageDataDto>> UpdateUser(UserPageDataUpdateDto dto, ClaimsPrincipal principal);
    }
}
