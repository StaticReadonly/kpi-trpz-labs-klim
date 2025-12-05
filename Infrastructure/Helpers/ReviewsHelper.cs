using BookingClinic.Application.Interfaces.Helpers;
using BookingClinic.Application.Interfaces.UnitOfWork;

namespace BookingClinic.Infrastructure.Helpers
{
    public class ReviewsHelper : IReviewsHelper
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserContextHelper _userContextHelper;

        public ReviewsHelper(
            IUnitOfWork unitOfWork,
            IUserContextHelper userContextHelper)
        {
            this._unitOfWork = unitOfWork;
            this._userContextHelper = userContextHelper;
        }

        public bool CanUserWriteReview(Guid doctorId)
        {
            var idClaim = _userContextHelper.UserId;

            if (idClaim == null)
            {
                return false;
            }

            var idValue = idClaim.Value;

            var resHasReviews = !_unitOfWork.DoctorReviews.GetDoctorPatientReviews(doctorId, idValue).Any();
            var resHasAppointments = _unitOfWork.Appointments.GetPatientDoctorAppointments(idValue, doctorId).Any();

            return resHasReviews && resHasAppointments;
        }
    }
}