namespace BookingClinic.Services.Data.User
{
    public class UserPageDataDto
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Role { get; set; }
        public string Email { get; set; }
        public string? Phone { get; set; }
        public string? ProfilePicture { get; set; }
    }
}
