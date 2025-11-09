namespace BookingClinic.Application.Data.Doctor
{
    public class DoctorDataDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Clinic { get; set; }
        public string Speciality { get; set; }
        public string? ProfilePicture { get; set; }
        public double Rating { get; set; }
        public bool CanWriteReview { get; set; }
        public List<Tuple<string, IEnumerable<string>>> Appointments { get; set; }
    }
}
