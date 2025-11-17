using BookingClinic.Application.Interfaces.Helpers;
using BookingClinic.Application.Interfaces.UnitOfWork;
using System.Security.Claims;

namespace BookingClinic.Infrastructure.Helpers
{
    public class ReviewsHelper : IReviewsHelper
    {
        private readonly IUnitOfWork _unitOfWork;

        public ReviewsHelper(
            IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }

        public bool CanUserWriteReview(Guid doctorId, ClaimsPrincipal principal)
        {
            var idClaim = principal.FindFirst(c => c.Type == ClaimTypes.NameIdentifier);

            if (idClaim == null)
            {
                return false;
            }

            var idValue = Guid.Parse(idClaim.Value);

            var resHasReviews = !_unitOfWork.DoctorReviews.GetDoctorPatientReviews(doctorId, idValue).Any();
            var resHasAppointments = _unitOfWork.Appointments.GetPatientDoctorAppointments(idValue, doctorId).Any();

            return resHasReviews && resHasAppointments;
        }
    }
}