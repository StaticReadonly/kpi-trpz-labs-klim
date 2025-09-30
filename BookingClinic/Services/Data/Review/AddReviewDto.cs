namespace BookingClinic.Services.Data.Review
{
    public class AddReviewDto
    {
        public Guid DoctorId { get; set; }
        public string Text { get; set; }
        public int Rating { get; set; }
    }
}
