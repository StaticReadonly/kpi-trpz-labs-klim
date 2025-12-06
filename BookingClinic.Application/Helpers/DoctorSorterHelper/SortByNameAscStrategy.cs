using BookingClinic.Application.Data.Doctor;

namespace BookingClinic.Application.Helpers.DoctorSorterHelper
{
    public class SortByNameAscStrategy : IDoctorSorterStrategy
    {
        public IEnumerable<SearchDoctorResDto> Sort(IEnumerable<SearchDoctorResDto> items) =>
            items.OrderBy(x => x.Name);
    }
}
