namespace BookingClinic.Services.Data.Appointment
{
    public class DoctorAppointmentDto
    {
        public Guid Id { get; set; }
        public Guid PatientId { get; set; }
        public string PatientNameSurname { get; set; }
        public string? PatientProfilePicture { get; set; }
        public DateTime DateTime { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsCanceled { get; set; }
        public bool IsFinished { get; set; }
        public string? Results { get; set; }
    }
}
