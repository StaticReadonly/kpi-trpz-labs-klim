using BookingClinic.Application.Data.User;

namespace BookingClinic.Application.Interfaces
{
    public interface IFileStorage
    {
        Task<string> SaveUserPhotoAsync(UserPictureDto file, Guid userId);
    }
}
