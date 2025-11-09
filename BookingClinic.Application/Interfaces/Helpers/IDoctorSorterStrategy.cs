using BookingClinic.Application.Data.Doctor;

namespace BookingClinic.Application.Helpers
{
    public interface IDoctorSorterStrategy
    {
        IEnumerable<SearchDoctorResDto> Sort(IEnumerable<SearchDoctorResDto> items);
    }
}
