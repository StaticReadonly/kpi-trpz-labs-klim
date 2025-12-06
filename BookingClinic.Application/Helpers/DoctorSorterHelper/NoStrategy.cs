using BookingClinic.Application.Data.Doctor;

namespace BookingClinic.Application.Helpers.DoctorSorterHelper
{
    public class NoStrategy : IDoctorSorterStrategy
    {
        public IEnumerable<SearchDoctorResDto> Sort(IEnumerable<SearchDoctorResDto> items) => items;
    }
}
