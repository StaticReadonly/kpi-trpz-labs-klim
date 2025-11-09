using BookingClinic.Application.Data.Doctor;

namespace BookingClinic.Application.Interfaces.Helpers
{
    public interface IDoctorSorter
    {
        IEnumerable<SearchDoctorResDto> Sort(IEnumerable<SearchDoctorResDto> items);
        void SetStrategy(string? strategy);
    }
}
