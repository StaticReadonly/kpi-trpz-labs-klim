using System.ComponentModel.DataAnnotations;

namespace BookingClinic.Domain.Entities
{
    public class Clinic
    {
        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string? Picture { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public string Building { get; set; }
        public DateTime CreatedDate { get; set; }
        public ICollection<Doctor> Doctors { get; set; } 
    }
}
