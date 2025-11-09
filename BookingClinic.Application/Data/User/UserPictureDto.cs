namespace BookingClinic.Application.Data.User
{
    public class UserPictureDto
    {
        public UserPictureDto(string fileName, Stream fileStream)
        {
            this.FileName = fileName;
            this.FileStream = fileStream;
        }

        public string FileName { get; set; }
        public Stream FileStream { get; set; }
    }
}
