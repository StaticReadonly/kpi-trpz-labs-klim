using BookingClinic.Application.Data.Doctor;

namespace BookingClinic.Application.Data.Appointment
{
    public class AppointmentDoctorDataDto : DoctorDataDto
    {
        public Guid PatientId { get; set; }
    }
}
