using BookingClinic.Services.Data.Doctor;

namespace BookingClinic.Services.Helpers.DoctorsSortingHelper.DoctorSorterStrategies
{
    public class SortByNameAscStrategy : IDoctorSorterStrategy
    {
        public IEnumerable<SearchDoctorResDto> Sort(IEnumerable<SearchDoctorResDto> items) =>
            items.OrderBy(x => x.Name);
    }
}
