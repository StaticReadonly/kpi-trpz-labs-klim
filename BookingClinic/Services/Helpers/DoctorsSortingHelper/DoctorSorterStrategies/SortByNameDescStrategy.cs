using BookingClinic.Services.Data.Doctor;

namespace BookingClinic.Services.Helpers.DoctorsSortingHelper.DoctorSorterStrategies
{
    public class SortByNameDescStrategy : IDoctorSorterStrategy
    {
        public IEnumerable<SearchDoctorResDto> Sort(IEnumerable<SearchDoctorResDto> items) =>
            items.OrderByDescending(x => x.Name);
    }
}
