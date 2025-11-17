using BookingClinic.Application.Interfaces.Helpers;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace BookingClinic.Infrastructure.Helpers
{
    public class UserContextHelper : IUserContextHelper
    {
        private readonly IHttpContextAccessor _contextAccessor;

        public UserContextHelper(
            IHttpContextAccessor contextAccessor)
        {
            this._contextAccessor = contextAccessor;
        }

        public Guid? UserId
        {
            get
            {
                var idClaim = Principal?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(idClaim)) return null;
                if (Guid.TryParse(idClaim, out var g)) return g;
                return null;
            }
        }

        public bool IsAuthenticated => Principal?.Identity?.IsAuthenticated ?? false;

        public ClaimsPrincipal? Principal => _contextAccessor.HttpContext?.User;

        public bool IsAdmin => Principal?.IsInRole("Admin") ?? false;

        public bool IsDoctor => Principal?.IsInRole("Doctor") ?? false;

        public bool IsPatient => Principal?.IsInRole("Patient") ?? false;

        public string? GetClaim(string claimType) => Principal?.FindFirst(claimType)?.Value;
    }
}
