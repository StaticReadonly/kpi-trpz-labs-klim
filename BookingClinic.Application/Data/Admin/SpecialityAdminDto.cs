using System;

namespace BookingClinic.Application.Data.Admin
{
    public class SpecialityAdminDto
    {
        public Guid? Id { get; set; }
        public string Name { get; set; } = null!;
    }
}