using BookingClinic.Services.Data.Doctor;

namespace BookingClinic.Services.Helpers.DoctorsSortingHelper.DoctorSorter
{
    public interface IDoctorSorter
    {
        IEnumerable<SearchDoctorResDto> Sort(IEnumerable<SearchDoctorResDto> items);
        void SetStrategy(string? strategy);
    }
}
