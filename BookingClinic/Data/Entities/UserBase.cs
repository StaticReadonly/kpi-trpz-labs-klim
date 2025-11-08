using BookingClinic.Services.Visitor;
using System.ComponentModel.DataAnnotations;

namespace BookingClinic.Data.Entities
{
    public class UserBase
    {
        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string? Phone { get; set; }
        public string? ProfilePicture { get; set; }
        public string PasswordHash { get; set; }
        public string Role { get; set; }
        public ICollection<Appointment> ClientAppointments { get; set; }
        public ICollection<DoctorReview> ClientReviews { get; set; }

        public virtual void AcceptVisitor(IUserVisitor visitor)
        {

        }
    }
}
