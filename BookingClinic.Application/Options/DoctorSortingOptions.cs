using BookingClinic.Application.Helpers;

namespace BookingClinic.Application.Options
{
    public class DoctorSortingOptions
    {
        public Dictionary<string, IDoctorSorterStrategy> Strategies { get; set; }
    }
}
