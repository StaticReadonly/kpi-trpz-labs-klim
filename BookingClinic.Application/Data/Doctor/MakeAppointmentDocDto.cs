namespace BookingClinic.Application.Data.Doctor
{
    public class MakeAppointmentDocDto
    {
        public Guid PatientId { get; set; }
        public string AppointmentDay { get; set; }
        public string AppointmentTime { get; set; }
    }
}
