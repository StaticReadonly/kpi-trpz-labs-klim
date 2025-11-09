using BookingClinic.Application.Common;
using BookingClinic.Application.Data.Doctor;
using BookingClinic.Application.Interfaces.Repositories;
using BookingClinic.Application.Interfaces.Services;
using BookingClinic.Domain.Interfaces;
using Mapster;

namespace BookingClinic.Application.Services
{
    public class DoctorService : IDoctorService
    {
        private readonly IUserRepository _userRepository;
        private readonly IAppointmentDomainService _appointmentDomainService;

        public DoctorService(
            IUserRepository userRepository, 
            IAppointmentDomainService appointmentDomainService)
        {
            _userRepository = userRepository;
            _appointmentDomainService = appointmentDomainService;
        }

        public ServiceResult<DoctorDataDto> GetDoctorData(Guid doctorId)
        {
            var doctor = _userRepository.GetDoctorByIdWithAppClinicSpeciality(doctorId);

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
