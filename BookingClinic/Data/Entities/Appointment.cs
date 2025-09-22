using System.ComponentModel.DataAnnotations;

namespace BookingClinic.Data.Entities
{
    public class Appointment 
    {
        [Key]
        public Guid Id { get; set; }
        public Guid DoctorId { get; set; }
        public Doctor Doctor { get; set; }
        public Guid PatientId { get; set; }
        public UserBase Patient { get; set; }
        public bool IsCanceled { get; set; }
        public bool IsFinished { get; set; }
        public string? Results { get; set; }
        public string Address { get; set; }
        public DateTime DateTime { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
