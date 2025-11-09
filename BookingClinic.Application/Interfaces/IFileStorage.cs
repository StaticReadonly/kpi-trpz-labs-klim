using BookingClinic.Application.Data.User;

namespace BookingClinic.Application.Interfaces
{
    public interface IFileStorage
    {
        Task SaveUserPhotoAsync(UserPictureDto file, Guid userId);
    }
}
