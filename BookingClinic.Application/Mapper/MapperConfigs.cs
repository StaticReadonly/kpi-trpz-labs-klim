using BookingClinic.Application.Data.Admin;
using BookingClinic.Application.Data.Appointment;
using BookingClinic.Application.Data.Doctor;
using BookingClinic.Application.Data.Review;
using BookingClinic.Application.Data.Visitor;
using BookingClinic.Domain.Entities;
using Mapster;

namespace BookingClinic.Services.Mapper
{
    public static class MapperConfigs
    {
        public static void RegisterMappings()
        {
            TypeAdapterConfig<Doctor, SearchDoctorResDto>.NewConfig()
                .Map(dest => dest.Clinic, src => src.Clinic.Name)
                .Map(dest => dest.Speciality, src => src.Speciality.Name);

            TypeAdapterConfig<Doctor, DoctorDataDto>.NewConfig()
                .Map(dest => dest.Clinic, src => $"{src.Clinic.Name}, {src.Clinic.City} {src.Clinic.Street} {src.Clinic.Building}")
                .Map(dest => dest.Speciality, src => src.Speciality.Name);

            TypeAdapterConfig<DoctorReview, ReviewDataDto>.NewConfig()
                .Map(dest => dest.OwnerName, src => src.Patient.Name);

            TypeAdapterConfig<Appointment, DoctorAppointmentDto>.NewConfig()
                .Map(dest => dest.PatientNameSurname, src => $"{src.Patient.Name} {src.Patient.Surname}")
                .Map(dest => dest.PatientProfilePicture, src => src.Patient.ProfilePicture);

            TypeAdapterConfig<Appointment, PatientAppointmentDto>.NewConfig()
                .Map(dest => dest.DoctorNameSurname, src => $"{src.Doctor.Name} {src.Doctor.Surname}")
                .Map(dest => dest.DoctorProfilePicture, src => src.Doctor.ProfilePicture);

            TypeAdapterConfig<Patient, VisitablePatientModel>.NewConfig()
                .Map(dest => dest.AppointmetsCount, src => src.ClientAppointments.Count)
                .Map(dest => dest.FinishedAppointmentsCount, src => src.ClientAppointments.Count(a => a.IsFinished))
                .Map(dest => dest.CanceledAppointmentsCount, src => src.ClientAppointments.Count(a => a.IsCanceled));

            TypeAdapterConfig<Doctor, VisitableDoctorModel>.NewConfig()
                .Map(dest => dest.Speciality, src => src.Speciality.Name)
                .Map(dest => dest.Clinic, src => src.Clinic.Name)
                .Map(dest => dest.AppointmetsCount, src => src.DoctorAppointments.Count)
                .Map(dest => dest.FinishedAppointmentsCount, src => src.DoctorAppointments.Count(a => a.IsFinished))
                .Map(dest => dest.CanceledAppointmentsCount, src => src.DoctorAppointments.Count(a => a.IsCanceled));

            TypeAdapterConfig<Doctor, UserAdminDto>.NewConfig()
                .Map(dest => dest.ClinicId, src => src.ClinicId)
                .Map(dest => dest.SpecialityId, src => src.SpecialityId);
        }
    }
}
