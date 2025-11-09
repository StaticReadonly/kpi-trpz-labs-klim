using System;
using System.ComponentModel.DataAnnotations;

namespace BookingClinic.Application.Data.Admin
{
    public class ClinicAdminDto
    {
        public Guid? Id { get; set; }

        [Required, StringLength(200)]
        public string Name { get; set; } = null!;

        [Required, StringLength(100)]
        public string City { get; set; } = null!;

        [Required, StringLength(200)]
        public string Street { get; set; } = null!;

        [Required, StringLength(50)]
        public string Building { get; set; } = null!;
    }
}