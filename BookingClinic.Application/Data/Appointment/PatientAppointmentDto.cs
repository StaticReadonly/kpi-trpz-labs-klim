namespace BookingClinic.Application.Data.Appointment
{
    public class PatientAppointmentDto
    {
        public Guid Id { get; set; }
        public Guid DoctorId { get; set; }
        public string DoctorNameSurname { get; set; }
        public string? DoctorProfilePicture { get; set; }
        public string Address { get; set; }
        public DateTime DateTime { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsCanceled { get; set; }
        public bool IsFinished { get; set; }
        public string? Results { get; set; }
    }
}
