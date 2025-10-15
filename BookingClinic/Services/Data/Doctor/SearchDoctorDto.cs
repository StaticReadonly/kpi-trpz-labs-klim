namespace BookingClinic.Services.Data.Doctor
{
    public class SearchDoctorDto
    {
        public string? Query { get; set; }
        public string? Speciality { get; set; }
        public string? Clinic { get; set; }
        public string? OrderBy { get; set; }
    }
}
