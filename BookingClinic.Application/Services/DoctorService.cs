using BookingClinic.Application.Common;
using BookingClinic.Application.Data.Doctor;
using BookingClinic.Application.Interfaces.Helpers;
using BookingClinic.Application.Interfaces.Services;
using BookingClinic.Application.Interfaces.UnitOfWork;
using BookingClinic.Domain.Interfaces;
using Mapster;

namespace BookingClinic.Application.Services
{
    public class DoctorService : IDoctorService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserContextHelper _userContextHelper;
        private readonly IAppointmentDomainService _appointmentDomainService;
        private readonly IReviewsHelper _reviewsHelper;

        public DoctorService(
            IUnitOfWork unitOfWork,
            IAppointmentDomainService appointmentDomainService,
            IUserContextHelper userContextHelper,
            IReviewsHelper reviewsHelper)
        {
            this._unitOfWork = unitOfWork;
            this._appointmentDomainService = appointmentDomainService;
            this._userContextHelper = userContextHelper;
            this._reviewsHelper = reviewsHelper;
        }

        public ServiceResult<DoctorDataDto> GetDoctorData(Guid doctorId)
        {
            var doctor = _unitOfWork.Users.GetDoctorByIdWithAppClinicSpeciality(doctorId);

            if (doctor == null)
            {
                return ServiceResult<DoctorDataDto>.Failure(ServiceError.DoctorNotFound());
            }

            var res = doctor.Adapt<DoctorDataDto>();

            res.Appointments = _appointmentDomainService.GetAppointments(doctor);
            res.Rating = doctor.DoctorReviews.Select(r => r.Rating).DefaultIfEmpty(0).Average();

            if (_userContextHelper.IsPatient || _userContextHelper.IsAdmin)
            {
                res.CanWriteReview = _reviewsHelper.CanUserWriteReview(doctorId);
            }

            return ServiceResult<DoctorDataDto>.Success(res);
        }
    }
}
