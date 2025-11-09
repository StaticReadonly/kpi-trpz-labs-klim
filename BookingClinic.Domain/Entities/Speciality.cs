using System.ComponentModel.DataAnnotations;

namespace BookingClinic.Domain.Entities
{
    public class Speciality
    {
        [Key]
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public ICollection<Doctor> Doctors { get; set; }
    }
}
