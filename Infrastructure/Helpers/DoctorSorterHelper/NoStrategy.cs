using BookingClinic.Application.Data.Doctor;
using BookingClinic.Application.Helpers;

namespace BookingClinic.Infrastructure.Helpers.DoctorSorterHelper
{
    public class NoStrategy : IDoctorSorterStrategy
    {
        public IEnumerable<SearchDoctorResDto> Sort(IEnumerable<SearchDoctorResDto> items) => items;
    }
}
