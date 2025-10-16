using BookingClinic.Data.Repositories.UserRepository;
using BookingClinic.Services.Data.Doctor;
using BookingClinic.Services.Helpers.AppointmentHelper;
using BookingClinic.Services.NotificationService.AdminNotificationService;
using Mapster;

namespace BookingClinic.Services.Doctor
{
    public class DoctorService : IDoctorService
    {
        private readonly IUserRepository _userRepository;
        private readonly IAppointmentHelper _appointmentHelper;
        private readonly IAdminAlertQueue _alertQueue;

        public DoctorService(
            IUserRepository userRepository,
            IAppointmentHelper appointmentHelper,
            IAdminAlertQueue alertQueue)
        {
            _userRepository = userRepository;
            _appointmentHelper = appointmentHelper;
            _alertQueue = alertQueue;
        }

        public async Task<ServiceResult<DoctorDataDto>> GetDoctorData(Guid doctorId)
        {
            BookingClinic.Data.Entities.Doctor? doctor = null;
            try
            {
                doctor = _userRepository.GetDoctorByIdWithAppClinicSpeciality(doctorId);
            }
            catch(Exception)
            {
                await _alertQueue.EnqueueAsync(
                    new AdminAlert("Db error", "Unable to retrieve data from db"));
            }

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
