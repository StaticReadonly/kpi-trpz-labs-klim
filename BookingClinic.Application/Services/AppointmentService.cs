using BookingClinic.Application.Common;
using BookingClinic.Application.Data.Appointment;
using BookingClinic.Application.Data.Doctor;
using BookingClinic.Application.Interfaces.Helpers;
using BookingClinic.Application.Interfaces.Services;
using BookingClinic.Application.Interfaces.UnitOfWork;
using Mapster;
using System.Globalization;

namespace BookingClinic.Application.Services
{
    public class AppointmentService : IAppointmentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserContextHelper _userContextHelper;

        public AppointmentService(
            IUnitOfWork unitOfWork,
            IUserContextHelper userContextHelper)
        {
            this._unitOfWork = unitOfWork;
            this._userContextHelper = userContextHelper;
        }

        public async Task<ServiceResult> CreateAppointment(MakeAppointmentDto dto)
        {
            var id = _userContextHelper.UserId!.Value;
            var doctor = _unitOfWork.Users.GetDoctorById(dto.DoctorId);

            if (doctor == null)
            {
                return ServiceResult.Failure(ServiceError.DoctorNotFound());
            }

            var clinic = doctor.Clinic;

            var times = dto.AppointmentTime.Split('-');
            var hoursMinutes = times[0].Split(':');
            var hours = int.Parse(hoursMinutes[0]);
            var minutes = int.Parse(hoursMinutes[1]);
            DateTime dateTime = DateTime.ParseExact(dto.AppointmentDay.Split(',')[1].Trim(), "dd.MM.yyyy", CultureInfo.InvariantCulture);
            dateTime = DateTime.SpecifyKind(dateTime.AddHours(hours).AddMinutes(minutes), DateTimeKind.Utc);

            var app = _unitOfWork.Appointments.GetByDateTime(dateTime);

            if (app != null)
            {
                return ServiceResult.Failure(ServiceError.AppointmentAlreadyExists());
            }

            var appointment = new Domain.Entities.Appointment()
            {
                Id = Guid.NewGuid(),
                PatientId = id,
                DoctorId = dto.DoctorId,
                CreatedAt = DateTime.UtcNow,
                DateTime = dateTime,
                Address = $"{clinic.Name}, {clinic.City} {clinic.Street} {clinic.Building}"
            };

            _unitOfWork.Appointments.AddEntity(appointment);

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

        public async Task<ServiceResult> CreateAppointmentDoctor(MakeAppointmentDocDto dto)
        {
            if (!_userContextHelper.IsDoctor)
            {
                return ServiceResult.Failure(ServiceError.Unauthorized());
            }

            var id = _userContextHelper.UserId!.Value;
            var doctor = _unitOfWork.Users.GetDoctorById(id);

            if (doctor == null)
            {
                return ServiceResult.Failure(ServiceError.DoctorNotFound());
            }

            var clinic = doctor.Clinic;

            var times = dto.AppointmentTime.Split('-');
            var hoursMinutes = times[0].Split(':');
            var hours = int.Parse(hoursMinutes[0]);
            var minutes = int.Parse(hoursMinutes[1]);
            DateTime dateTime = DateTime.ParseExact(dto.AppointmentDay.Split(',')[1].Trim(), "dd.MM.yyyy", CultureInfo.InvariantCulture);
            dateTime = DateTime.SpecifyKind(dateTime.AddHours(hours).AddMinutes(minutes), DateTimeKind.Utc);

            var app = _unitOfWork.Appointments.GetByDateTime(dateTime);

            if (app != null)
            {
                return ServiceResult.Failure(ServiceError.AppointmentAlreadyExists());
            }

            var appointment = new Domain.Entities.Appointment()
            {
                Id = Guid.NewGuid(),
                PatientId = dto.PatientId,
                DoctorId = id,
                CreatedAt = DateTime.UtcNow,
                DateTime = dateTime,
                Address = $"{clinic.Name}, {clinic.City} {clinic.Street} {clinic.Building}"
            };

            _unitOfWork.Appointments.AddEntity(appointment);

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

        public ServiceResult<List<PatientAppointmentDto>> GetPatientAppointments()
        {
            var id = _userContextHelper.UserId!.Value;

            var res = _unitOfWork.Appointments.GetPatientAppointments(id).ToList();

            var appointments = res.Adapt<List<PatientAppointmentDto>>();
            return ServiceResult<List<PatientAppointmentDto>>.Success(appointments);
        }

        public ServiceResult<List<DoctorAppointmentDto>> GetDoctorAppointments()
        {
            var id = _userContextHelper.UserId!.Value;

            var res = _unitOfWork.Appointments.GetDoctorAppointments(id, DateTime.UtcNow.Date);

            var appointments = res.Adapt<List<DoctorAppointmentDto>>();
            return ServiceResult<List<DoctorAppointmentDto>>.Success(appointments);
        }

        public async Task<ServiceResult> CancelAppointment(Guid appId)
        {
            var id = _userContextHelper.UserId!.Value;
            var appointment = _unitOfWork.Appointments.GetById(appId);

            if (appointment == null || (appointment.PatientId != id && !_userContextHelper.IsAdmin))
            {
                return ServiceResult.Failure(ServiceError.AppointmentNotFound());
            }

            appointment.IsCanceled = true;
            _unitOfWork.Appointments.UpdateEntity(appointment);

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

        public async Task<ServiceResult> FinishAppointment(FinishAppointmentDto dto)
        {
            var id = _userContextHelper.UserId!.Value;
            var appointment = _unitOfWork.Appointments.GetById(dto.Id);

            if (appointment == null || appointment.DoctorId != id)
            {
                return ServiceResult.Failure(ServiceError.AppointmentNotFound());
            }

            appointment.IsFinished = true;
            appointment.Results = dto.Results;
            _unitOfWork.Appointments.UpdateEntity(appointment);

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
    }
}
