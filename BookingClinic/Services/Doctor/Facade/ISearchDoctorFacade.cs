using BookingClinic.Services.Data.Doctor;

namespace BookingClinic.Services.Doctor.Facade
{
    public interface ISearchDoctorFacade
    {
        SearchDoctorIndexResult SearchFordoctors(SearchDoctorDto dto, int page);
    }
}
