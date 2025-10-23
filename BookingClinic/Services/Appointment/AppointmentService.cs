using BookingClinic.Data.Repositories.AppointmentRepository;
using BookingClinic.Data.Repositories.UserRepository;
using BookingClinic.Services.AppointmentObserver.Subject;
using BookingClinic.Services.Data.Appointment;
using BookingClinic.Services.Data.Doctor;
using Mapster;
using System.Globalization;
using System.Security.Claims;

namespace BookingClinic.Services.Appointment
{
    public class AppointmentService : IAppointmentService
    {
        private readonly IAppointmentSubject _appointmentSubject;
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly IUserRepository _userRepository;

        public AppointmentService(
            IAppointmentRepository appointmentRepository,
            IUserRepository userRepository,
            IAppointmentSubject appointmentSubject)
        {
            _appointmentRepository = appointmentRepository;
            _userRepository = userRepository;
            _appointmentSubject = appointmentSubject;
        }

        public async Task<ServiceResult<object>> CreateAppointment(MakeAppointmentDto dto, ClaimsPrincipal principal)
        {
            var idClaim = principal.FindFirst(c => c.Type == ClaimTypes.NameIdentifier);
            var id = Guid.Parse(idClaim.Value);

            var doctor = _userRepository.GetDoctorById(dto.DoctorId);

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
            DateTime dateTime = DateTime.ParseExact(dto.AppointmentDay.Split(',')[1].Trim(), "dd.MM.yyyy", CultureInfo.InvariantCulture);
            dateTime = DateTime.SpecifyKind(dateTime.AddHours(hours).AddMinutes(minutes), DateTimeKind.Utc);

            var app = _appointmentRepository.GetByDateTime(dateTime);

            if (app != null)
            {
                return ServiceResult<object>.Failure(
                    new List<ServiceError>() { ServiceError.AppointmentAlreadyExists() });
            }

            var user = _userRepository.GetById(id)!;
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
                await _appointmentSubject.NotifyAsync(appointment, user.Email);

                return ServiceResult<object>.Success(null);
            }
            catch (Exception)
            {
                return ServiceResult<object>.Failure(
                    new List<ServiceError>() { ServiceError.UnexpectedError() });
            }
        }

        public async Task<ServiceResult<object>> CreateAppointmentDoctor(MakeAppointmentDocDto dto, ClaimsPrincipal principal)
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
            DateTime dateTime = DateTime.ParseExact(dto.AppointmentDay.Split(',')[1].Trim(), "dd.MM.yyyy", CultureInfo.InvariantCulture);
            dateTime = DateTime.SpecifyKind(dateTime.AddHours(hours).AddMinutes(minutes), DateTimeKind.Utc);

            var app = _appointmentRepository.GetByDateTime(dateTime);

            if (app != null)
            {
                return ServiceResult<object>.Failure(
                    new List<ServiceError>() { ServiceError.AppointmentAlreadyExists() });
            }

            var user = _userRepository.GetById(dto.PatientId)!;
            var appointment = new BookingClinic.Data.Entities.Appointment()
            {
                Id = Guid.NewGuid(),
                PatientId = dto.PatientId,
                DoctorId = id,
                CreatedAt = DateTime.UtcNow,
                DateTime = dateTime,
                Address = $"{clinic.Name}, {clinic.City} {clinic.Street} {clinic.Building}"
            };

            _appointmentRepository.AddEntity(appointment);

            try
            {
                await _appointmentRepository.SaveChangesAsync();
                await _appointmentSubject.NotifyAsync(appointment, user.Email);

                return ServiceResult<object>.Success(null);
            }
            catch (Exception)
            {
                return ServiceResult<object>.Failure(
                    new List<ServiceError>() { ServiceError.UnexpectedError() });
            }
        }

        public ServiceResult<List<PatientAppointmentDto>> GetPatientAppointments(ClaimsPrincipal principal)
        {
            var idClaim = principal.FindFirst(c => c.Type == ClaimTypes.NameIdentifier);
            Guid id = Guid.Parse(idClaim.Value);

            var res = _appointmentRepository.GetPatientAppointments(id).ToList();

            var appointments = res.Adapt<List<PatientAppointmentDto>>();
            return ServiceResult<List<PatientAppointmentDto>>.Success(appointments);
        }

        public ServiceResult<List<DoctorAppointmentDto>> GetDoctorAppointments(ClaimsPrincipal principal)
        {
            var idClaim = principal.FindFirst(c => c.Type == ClaimTypes.NameIdentifier);
            Guid id = Guid.Parse(idClaim.Value);

            var res = _appointmentRepository.GetDoctorAppointments(id, DateTime.UtcNow.Date);

            var appointments = res.Adapt<List<DoctorAppointmentDto>>();
            return ServiceResult<List<DoctorAppointmentDto>>.Success(appointments);
        }

        public async Task<ServiceResult<object>> CancelAppointment(Guid appId, ClaimsPrincipal principal)
        {
            var idClaim = principal.FindFirst(c => c.Type == ClaimTypes.NameIdentifier);
            var id = Guid.Parse(idClaim.Value);

            var appointment = _appointmentRepository.GetById(appId);

            if (appointment == null)
            {
                return ServiceResult<object>.Failure(
                    new List<ServiceError>() { ServiceError.AppointmentNotFound() });
            }
            if (id != appointment.PatientId)
            {
                return ServiceResult<object>.Failure(
                    new List<ServiceError>() { ServiceError.AppointmentNotFound() });
            }

            appointment.IsCanceled = true;
            var user = _userRepository.GetById(id)!;
            _appointmentRepository.UpdateEntity(appointment);

            try
            {
                await _appointmentRepository.SaveChangesAsync();
                await _appointmentSubject.NotifyAsync(appointment, user.Email);
                return ServiceResult<object>.Success(null);
            }
            catch (Exception)
            {
                return ServiceResult<object>.Failure(
                    new List<ServiceError>() { ServiceError.UnexpectedError() });
            }
        }

        public async Task<ServiceResult<object>> FinishAppointment(FinishAppointmentDto dto, ClaimsPrincipal principal)
        {
            var idClaim = principal.FindFirst(c => c.Type == ClaimTypes.NameIdentifier);
            var id = Guid.Parse(idClaim.Value);

            var appointment = _appointmentRepository.GetById(dto.Id);

            if (appointment == null)
            {
                return ServiceResult<object>.Failure(
                    new List<ServiceError>() { ServiceError.AppointmentNotFound() });
            }
            if (appointment.DoctorId != id)
            {
                return ServiceResult<object>.Failure(
                    new List<ServiceError>() { ServiceError.AppointmentNotFound() });
            }

            var user = _userRepository.GetById(appointment.PatientId)!;
            appointment.IsFinished = true;
            appointment.Results = dto.Results;
            _appointmentRepository.UpdateEntity(appointment);

            try
            {
                await _appointmentRepository.SaveChangesAsync();
                await _appointmentSubject.NotifyAsync(appointment, user.Email);
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
