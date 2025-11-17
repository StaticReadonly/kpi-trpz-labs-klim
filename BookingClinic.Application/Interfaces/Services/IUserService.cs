using BookingClinic.Application.Common;
using BookingClinic.Application.Data.Doctor;
using BookingClinic.Application.Data.User;
using System.Security.Claims;

namespace BookingClinic.Application.Interfaces.Services
{
    public interface IUserService
    {
        ServiceResult<IEnumerable<SearchDoctorResDto>> SearchDoctors(SearchDoctorDto dto);
        ServiceResult<ClaimsPrincipal> LoginUser(LoginUserDto dto);
        Task<ServiceResult<ClaimsPrincipal>> RegisterUser(RegisterUserDto dto);
        ServiceResult<UserPageDataDto> GetUserData();
        Task<ServiceResult<object>> UpdateUserPhoto(UserPictureDto file);
        Task<ServiceResult<UserPageDataDto>> UpdateUser(UserPageDataUpdateDto dto);
    }
}
