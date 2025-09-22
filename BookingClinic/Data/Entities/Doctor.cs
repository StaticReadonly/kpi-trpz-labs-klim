namespace BookingClinic.Data.Entities
{
    public class Doctor : UserBase
    {
        public ICollection<DoctorReview> DoctorReviews { get; set; }
        public ICollection<Appointment> DoctorAppointments { get; set; } 
        public Guid SpecialityId { get; set; }
        public Speciality Speciality { get; set; }
        public Guid ClinicId { get; set; }
        public Clinic Clinic { get; set; }
    }
}
