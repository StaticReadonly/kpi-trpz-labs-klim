using BookingClinic.Application.Data.Doctor;
using BookingClinic.Application.Helpers;

namespace BookingClinic.Infrastructure.Helpers.DoctorSorterHelper
{
    public class SortByNameAscStrategy : IDoctorSorterStrategy
    {
        public IEnumerable<SearchDoctorResDto> Sort(IEnumerable<SearchDoctorResDto> items) =>
            items.OrderBy(x => x.Name);
    }
}
