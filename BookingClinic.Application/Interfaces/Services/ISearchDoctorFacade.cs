using BookingClinic.Application.Common;
using BookingClinic.Application.Data.Doctor;

namespace BookingClinic.Application.Interfaces.Services
{
    public interface ISearchDoctorFacade
    {
        SearchDoctorIndexResult SearchForDoctors(SearchDoctorDto dto, int page);
    }
}
