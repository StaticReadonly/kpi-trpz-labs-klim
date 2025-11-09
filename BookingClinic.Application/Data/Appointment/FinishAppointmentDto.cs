namespace BookingClinic.Application.Data.Appointment
{
    public class FinishAppointmentDto
    {
        public Guid Id { get; set; }
        public Guid PatientId { get; set; }
        public string? Results { get; set; }
    }
}
