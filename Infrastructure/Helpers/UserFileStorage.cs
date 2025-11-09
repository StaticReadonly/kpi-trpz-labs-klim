using BookingClinic.Application.Data.User;
using BookingClinic.Application.Interfaces;
using Microsoft.AspNetCore.Hosting;

namespace BookingClinic.Infrastructure.Helpers
{
    public class UserFileStorage : IFileStorage
    {
        private readonly IWebHostEnvironment _webHostEnvironment;

        public UserFileStorage(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task SaveUserPhotoAsync(UserPictureDto file, Guid userId)
        {
            var wwwrootPath = _webHostEnvironment.WebRootPath;
            var dir = Path.Combine(wwwrootPath, "profiles", "users");

            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            var path = Path.Combine(dir, file.FileName);

            using var newFile = File.Create(path);
            await file.FileStream.CopyToAsync(newFile);
        }
    }
}
