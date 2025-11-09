namespace BookingClinic.Application.Data.User
{
    public class UserPictureDto
    {
        public UserPictureDto(string fileName, Stream fileStream)
        {
            this.FileName = fileName;
            this.FileStream = fileStream;
        }

        public string FileName { get; init; }
        public Stream FileStream { get; init; }
    }
}
