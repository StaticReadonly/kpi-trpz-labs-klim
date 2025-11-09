using BookingClinic.Application.Common;
using BookingClinic.Application.Data.Review;
using BookingClinic.Application.Interfaces.Repositories;
using BookingClinic.Application.Interfaces.Services;
using BookingClinic.Domain.Entities;
using Mapster;
using System.Security.Claims;

namespace BookingClinic.Application.Services
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

        public async Task<ServiceResult<object>> CreateReview(AddReviewDto dto, ClaimsPrincipal principal)
        {
            var userIdClaim = principal.FindFirst(c => c.Type == ClaimTypes.NameIdentifier);
            var userId = Guid.Parse(userIdClaim!.Value);

            DoctorReview rev = new()
            {
                Id = Guid.NewGuid(),
                DoctorId = dto.DoctorId,
                PatientId = userId,
                Rating = dto.Rating,
                Text = dto.Text
            };

            _reviewsRepository.AddEntity(rev);

            try
            {
                await _reviewsRepository.SaveChangesAsync();

                return ServiceResult<object>.Success(null);
            }
            catch (Exception)
            {
                return ServiceResult<object>.Failure(
                    new List<ServiceError>() { ServiceError.UnexpectedError() });
            }
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
