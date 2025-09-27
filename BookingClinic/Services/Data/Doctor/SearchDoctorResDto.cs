namespace BookingClinic.Services.Data.Doctor
{
    public class SearchDoctorResDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Clinic { get; set; }
        public string Speciality { get; set; }
        public string? ProfilePicture { get; set; }
    }
}
