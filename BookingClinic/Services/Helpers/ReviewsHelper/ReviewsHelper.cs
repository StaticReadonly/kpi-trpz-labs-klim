using BookingClinic.Data.Repositories.AppointmentRepository;
using BookingClinic.Data.Repositories.DoctorReviewRepository;
using System.Security.Claims;

namespace BookingClinic.Services.Helpers.ReviewsHelper
{
    public class ReviewsHelper : IReviewsHelper
    {
        private readonly IDoctorReviewRepository _reviewRepository;
        private readonly IAppointmentRepository _appointmentRepository;

        public ReviewsHelper(
            IDoctorReviewRepository reviewRepository, 
            IAppointmentRepository appointmentRepository)
        {
            _reviewRepository = reviewRepository;
            _appointmentRepository = appointmentRepository;
        }

        public bool CanUserWriteReview(Guid doctorId, ClaimsPrincipal principal)
        {
            var idClaim = principal.FindFirst(c => c.Type == ClaimTypes.NameIdentifier);

            if (idClaim == null)
            {
                return false;
            }

            var idValue = Guid.Parse(idClaim.Value);

            var resHasReviews = !_reviewRepository.GetDoctorPatientReviews(doctorId, idValue).Any();
            var resHasAppointments = _appointmentRepository.GetPatientDoctorAppointments(idValue, doctorId).Any();

            return resHasReviews && resHasAppointments;
        }
    }
}
