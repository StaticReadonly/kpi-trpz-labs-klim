using BookingClinic.Services.Data.Doctor;

namespace BookingClinic.Services.Doctor.Facade
{
    public interface ISearchDoctorFacade
    {
        SearchDoctorIndexResult SearchForDoctors(SearchDoctorDto dto, int page);
    }
}
