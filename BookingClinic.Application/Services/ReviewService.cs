using BookingClinic.Application.Common;
using BookingClinic.Application.Data.Review;
using BookingClinic.Application.Interfaces.Helpers;
using BookingClinic.Application.Interfaces.Services;
using BookingClinic.Application.Interfaces.UnitOfWork;
using BookingClinic.Domain.Entities;
using Mapster;

namespace BookingClinic.Application.Services
{
    public class ReviewService : IReviewService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserContextHelper _userContextHelper;

        public ReviewService(
            IUnitOfWork unitOfWork,
            IUserContextHelper userContextHelper)
        {
            this._unitOfWork = unitOfWork;
            this._userContextHelper = userContextHelper;
        }

        public async Task<ServiceResult> CreateReview(AddReviewDto dto)
        {
            var id = _userContextHelper.UserId!.Value;

            if (!_userContextHelper.IsPatient && !_userContextHelper.IsAdmin)
            {
                return ServiceResult.Failure(ServiceError.Unauthorized());
            }

            DoctorReview rev = new()
            {
                Id = Guid.NewGuid(),
                DoctorId = dto.DoctorId,
                PatientId = id,
                Rating = dto.Rating,
                Text = dto.Text
            };

            _unitOfWork.DoctorReviews.AddEntity(rev);

            try
            {
                await _unitOfWork.SaveChangesAsync();

                return ServiceResult.Success();
            }
            catch (Exception)
            {
                return ServiceResult.Failure(ServiceError.UnexpectedError());
            }
        }

        public async Task<ServiceResult> DeleteReview(DeleteReviewDto dto)
        {
            var id = _userContextHelper.UserId!.Value;

            if (!_userContextHelper.IsPatient && !_userContextHelper.IsAdmin)
            {
                return ServiceResult.Failure(ServiceError.Unauthorized());
            }

            var review = _unitOfWork.DoctorReviews.GetById(dto.ReviewId);

            if (review == null)
            {
                return ServiceResult.Failure(ServiceError.ReviewNotFound());
            }
            else if (review.PatientId != id)
            {
                return ServiceResult.Failure(ServiceError.Unauthorized());
            }

            _unitOfWork.DoctorReviews.DeleteEntity(review);

            try
            {
                await _unitOfWork.SaveChangesAsync();
                return ServiceResult.Success();
            }
            catch (Exception)
            {
                return ServiceResult.Failure(ServiceError.UnexpectedError());
            }
        }

        public ServiceResult<DoctorReviewsDto> GetDoctorReviews(Guid doctorId)
        {
            var doctor = _unitOfWork.Users.GetById(doctorId);

            if (doctor == null)
            {
                return ServiceResult<DoctorReviewsDto>.Failure(ServiceError.DoctorNotFound());
            }

            try
            {
                var reviews = _unitOfWork.DoctorReviews.GetDoctorsReviews(doctorId);

                var res = doctor.Adapt<DoctorReviewsDto>();
                res.Reviews = reviews.Adapt<IEnumerable<ReviewDataDto>>();
                res.Rating = res.Reviews.Select(r => r.Rating).DefaultIfEmpty(0).Average();

                return ServiceResult<DoctorReviewsDto>.Success(res);
            }
            catch (Exception)
            {
                return ServiceResult<DoctorReviewsDto>.Failure(ServiceError.UnexpectedError());
            }
        }
    }
}
