using BookingClinic.Data.Repositories.AppointmentRepository;
using BookingClinic.Data.Repositories.UserRepository;
using BookingClinic.Services.Data.Appointment;
using System.Globalization;
using System.Security.Claims;

namespace BookingClinic.Services.Appointment
{
    public class AppointmentService : IAppointmentService
    {
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly IUserRepository _userRepository;

        public AppointmentService(
            IAppointmentRepository appointmentRepository, 
            IUserRepository userRepository)
        {
            _appointmentRepository = appointmentRepository;
            _userRepository = userRepository;
        }

        public async Task<ServiceResult<object>> CreateAppointment(MakeAppointmentDto dto, ClaimsPrincipal principal)
        {
            var idClaim = principal.FindFirst(c => c.Type == ClaimTypes.NameIdentifier);
            var id = Guid.Parse(idClaim.Value);

            var doctor = _userRepository.GetDoctorById(id);

            if (doctor == null)
            {
                return ServiceResult<object>.Failure(
                    new List<ServiceError>() { ServiceError.DoctorNotFound() });
            }

            var clinic = doctor.Clinic;

            var times = dto.AppointmentTime.Split('-');
            var hoursMinutes = times[0].Split(':');
            var hours = int.Parse(hoursMinutes[0]);
            var minutes = int.Parse(hoursMinutes[1]);
            DateTime dateTime = DateTime.ParseExact(dto.AppointmentDay, "dd.MM.yyyy", CultureInfo.InvariantCulture);
            dateTime.AddHours(hours);
            dateTime.AddMinutes(minutes);

            var app = _appointmentRepository.GetByDateTime(dateTime);

            if (app != null)
            {
                return ServiceResult<object>.Failure(
                    new List<ServiceError>() { ServiceError.AppointmentAlreadyExists() });
            }

            var appointment = new BookingClinic.Data.Entities.Appointment()
            {
                Id = Guid.NewGuid(),
                PatientId = id,
                DoctorId = dto.DoctorId,
                CreatedAt = DateTime.UtcNow,
                DateTime = dateTime,
                Address = $"{clinic.Name}, {clinic.City} {clinic.Street} {clinic.Building}"
            };

            _appointmentRepository.AddEntity(appointment);

            try
            {
                await _appointmentRepository.SaveChangesAsync();

                return ServiceResult<object>.Success(null);
            }
            catch (Exception)
            {
                return ServiceResult<object>.Failure(
                    new List<ServiceError>() { ServiceError.UnexpectedError() });
            }
        }
    }
}
