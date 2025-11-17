using System.Security.Claims;

namespace BookingClinic.Application.Interfaces.Helpers
{
    public interface IUserContextHelper
    {
        Guid? UserId { get; }
        bool IsAuthenticated { get; }
        ClaimsPrincipal? Principal { get; }
        public string? GetClaim(string claimType);
        public bool IsAdmin { get; }
        public bool IsDoctor { get; }
        public bool IsPatient { get; }
    }
}
