using BookingClinic.Data.Entities;
using BookingClinic.Services.Data.Doctor;
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


        }
    }
}
