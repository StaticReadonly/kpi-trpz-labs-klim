using BookingClinic.Application.Data.Doctor;
using BookingClinic.Application.Helpers;

namespace BookingClinic.Infrastructure.Helpers.DoctorSorterHelper
{
    public class SortByNameDescStrategy : IDoctorSorterStrategy
    {
        public IEnumerable<SearchDoctorResDto> Sort(IEnumerable<SearchDoctorResDto> items) =>
            items.OrderByDescending(x => x.Name);
    }
}
