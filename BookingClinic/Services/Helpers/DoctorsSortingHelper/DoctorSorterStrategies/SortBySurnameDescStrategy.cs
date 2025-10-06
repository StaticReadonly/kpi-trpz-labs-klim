using BookingClinic.Services.Data.Doctor;

namespace BookingClinic.Services.Helpers.DoctorsSortingHelper.DoctorSorterStrategies
{
    public class SortBySurnameDescStrategy : IDoctorSorterStrategy
    {
        public IEnumerable<SearchDoctorResDto> Sort(IEnumerable<SearchDoctorResDto> items) =>
            items.OrderByDescending(x => x.Surname);
    }
}
