namespace BookingClinic.Application.Data.Review
{
    public class DoctorReviewsDto
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string? ProfilePicture { get; set; }
        public double Rating { get; set; }
        public IEnumerable<ReviewDataDto> Reviews { get; set; }
    }
}
