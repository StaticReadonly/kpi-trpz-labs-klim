using BookingClinic.Data.Entities;
using BookingClinic.Services.Data.Appointment;
using BookingClinic.Services.Data.Doctor;
using BookingClinic.Services.Data.Review;
using Mapster;

namespace BookingClinic.Services.Mapper
{
    public static class MapperConfigs
    {
        public static void RegisterMappings()
        {
            TypeAdapterConfig<BookingClinic.Data.Entities.Doctor, SearchDoctorResDto>.NewConfig()
                .Map(dest => dest.Clinic, src => src.Clinic.Name)
                .Map(dest => dest.Speciality, src => src.Speciality.Name);

            TypeAdapterConfig<BookingClinic.Data.Entities.Doctor, DoctorDataDto>.NewConfig()
                .Map(dest => dest.Clinic, src => $"{src.Clinic.Name}, {src.Clinic.City} {src.Clinic.Street} {src.Clinic.Building}")
                .Map(dest => dest.Speciality, src => src.Speciality.Name);

            TypeAdapterConfig<DoctorReview, ReviewDataDto>.NewConfig()
                .Map(dest => dest.OwnerName, src => src.Patient.Name);

            TypeAdapterConfig<BookingClinic.Data.Entities.Appointment, DoctorAppointmentDto>.NewConfig()
                .Map(dest => dest.PatientNameSurname, src => $"{src.Patient.Name} {src.Patient.Surname}")
                .Map(dest => dest.PatientProfilePicture, src => src.Patient.ProfilePicture);

            TypeAdapterConfig<BookingClinic.Data.Entities.Appointment, PatientAppointmentDto>.NewConfig()
                .Map(dest => dest.DoctorNameSurname, src => $"{src.Doctor.Name} {src.Doctor.Surname}")
                .Map(dest => dest.DoctorProfilePicture, src => src.Doctor.ProfilePicture);
        }
    }
}
