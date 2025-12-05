namespace BookingClinic.Application.Interfaces.Helpers
{
    public interface IReviewsHelper
    {
        bool CanUserWriteReview(Guid doctorId);
    }
}
