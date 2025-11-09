using System;
using System.ComponentModel.DataAnnotations;

namespace BookingClinic.Application.Data.Admin
{
    public class UserAdminDto
    {
        public Guid? Id { get; set; }

        [Required, StringLength(100)]
        public string Name { get; set; } = null!;

        [Required, StringLength(100)]
        public string Surname { get; set; } = null!;

        [Required, EmailAddress]
        public string Email { get; set; } = null!;

        [StringLength(20)]
        public string? Phone { get; set; }

        [Required, MinLength(6)]
        public string Password { get; set; } = null!;

        [Required, Compare("Password", ErrorMessage = "Passwords must match")]
        public string ConfirmPassword { get; set; } = null!;

        [Required]
        public string Role { get; set; } = "Patient";

        public Guid? ClinicId { get; set; }
        public Guid? SpecialityId { get; set; }
    }
}