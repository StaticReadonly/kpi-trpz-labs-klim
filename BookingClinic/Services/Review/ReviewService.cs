using BookingClinic.Data.Repositories.DoctorReviewRepository;
using BookingClinic.Data.Repositories.UserRepository;
using BookingClinic.Services.Data.Review;
using Mapster;

namespace BookingClinic.Services.Review
{
    public class ReviewService : IReviewService
    {
        private readonly IDoctorReviewRepository _reviewsRepository;
        private readonly IUserRepository _usersRepository;

        public ReviewService(
            IDoctorReviewRepository reviewsRepository, 
            IUserRepository usersRepository)
        {
            _reviewsRepository = reviewsRepository;
            _usersRepository = usersRepository;
        }

        public ServiceResult<DoctorReviewsDto> GetDoctorReviews(Guid doctorId)
        {
            var doctor = _usersRepository.GetById(doctorId);

            if (doctor == null)
            {
                return ServiceResult<DoctorReviewsDto>.Failure(
                    new List<ServiceError>() { ServiceError.DoctorNotFound() });
            }

            var reviews = _reviewsRepository.GetDoctorsReviews(doctorId);

            var res = doctor.Adapt<DoctorReviewsDto>();
            res.Reviews = reviews.Adapt<IEnumerable<ReviewDataDto>>();
            res.Rating = res.Reviews.Select(r => r.Rating).DefaultIfEmpty(0).Average();

            return ServiceResult<DoctorReviewsDto>.Success(res);
        }
    }
}
