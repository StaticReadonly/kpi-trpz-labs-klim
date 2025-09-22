using System.ComponentModel.DataAnnotations;

namespace BookingClinic.Data.Entities
{
    public class DoctorReview 
    {
        [Key]
        public Guid Id { get; set; }
        public Guid DoctorId { get; set; }
        public Doctor Doctor { get; set; }
        public Guid PatientId { get; set; }
        public UserBase Patient { get; set; }
        public int Rating { get; set; }
        public string? Text { get; set; }
    }
}
