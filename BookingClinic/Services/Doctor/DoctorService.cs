using BookingClinic.Data.Repositories.UserRepository;
using BookingClinic.Services.Data.Doctor;
using BookingClinic.Services.Helpers.AppointmentHelper;
using Mapster;

namespace BookingClinic.Services.Doctor
{
    public class DoctorService : IDoctorService
    {
        private readonly IUserRepository _userRepository;
        private readonly IAppointmentHelper _appointmentHelper;

        public DoctorService(
            IUserRepository userRepository, 
            IAppointmentHelper appointmentHelper)
        {
            _userRepository = userRepository;
            _appointmentHelper = appointmentHelper;
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

            res.Appointments = _appointmentHelper.GetAppointments(doctor);
            res.Rating = doctor.DoctorReviews.Select(r => r.Rating).DefaultIfEmpty(0).Average();

            return ServiceResult<DoctorDataDto>.Success(res);
        }
    }
}
