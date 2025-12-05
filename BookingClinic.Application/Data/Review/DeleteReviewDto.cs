namespace BookingClinic.Application.Data.Review
{
    public class DeleteReviewDto
    {
        public Guid DoctorId { get; set; }

        public Guid ReviewId { get; set; }
    }
}
