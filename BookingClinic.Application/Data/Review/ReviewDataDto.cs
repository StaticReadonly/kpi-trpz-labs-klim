namespace BookingClinic.Application.Data.Review
{
    public class ReviewDataDto
    {
        public Guid Id { get; set; }

        public Guid PatientId { get; set; }

        public string OwnerName { get; set; }

        public string? ProfilePicture { get; set; }

        public int Rating { get; set; }

        public string Text { get; set; }
    }
}
