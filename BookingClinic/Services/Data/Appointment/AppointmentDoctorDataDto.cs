using BookingClinic.Services.Data.Doctor;

namespace BookingClinic.Services.Data.Appointment
{
    public class AppointmentDoctorDataDto : DoctorDataDto
    {
        public Guid PatientId { get; set; }
    }
}
