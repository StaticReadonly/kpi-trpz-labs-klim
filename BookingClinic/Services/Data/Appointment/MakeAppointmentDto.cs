namespace BookingClinic.Services.Data.Appointment
{
    public class MakeAppointmentDto
    {
        public Guid DoctorId { get; set; }
        public string AppointmentDay { get; set; }
        public string AppointmentTime { get; set; }
    }
}
