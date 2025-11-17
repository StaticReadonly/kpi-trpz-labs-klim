using BookingClinic.Application.Common;
using BookingClinic.Application.Data.Doctor;
using BookingClinic.Application.Interfaces.Services;
using BookingClinic.Application.Interfaces.UnitOfWork;
using BookingClinic.Domain.Interfaces;
using Mapster;

namespace BookingClinic.Application.Services
{
    public class DoctorService : IDoctorService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAppointmentDomainService _appointmentDomainService;

        public DoctorService(
            IUnitOfWork unitOfWork,
            IAppointmentDomainService appointmentDomainService)
        {
            this._unitOfWork = unitOfWork;
            this._appointmentDomainService = appointmentDomainService;
        }

        public ServiceResult<DoctorDataDto> GetDoctorData(Guid doctorId)
        {
            var doctor = _unitOfWork.Users.GetDoctorByIdWithAppClinicSpeciality(doctorId);

            if (doctor == null)
            {
                return ServiceResult<DoctorDataDto>.Failure(
                    new List<ServiceError>() { ServiceError.DoctorNotFound() });
            }

            var res = doctor.Adapt<DoctorDataDto>();

            res.Appointments = _appointmentDomainService.GetAppointments(doctor);
            res.Rating = doctor.DoctorReviews.Select(r => r.Rating).DefaultIfEmpty(0).Average();

            return ServiceResult<DoctorDataDto>.Success(res);
        }
    }
}
