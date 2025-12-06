using BookingClinic.Application.Data.Doctor;

namespace BookingClinic.Application.Helpers.DoctorSorterHelper
{
    public class SortByNameDescStrategy : IDoctorSorterStrategy
    {
        public IEnumerable<SearchDoctorResDto> Sort(IEnumerable<SearchDoctorResDto> items) =>
            items.OrderByDescending(x => x.Name);
    }
}
