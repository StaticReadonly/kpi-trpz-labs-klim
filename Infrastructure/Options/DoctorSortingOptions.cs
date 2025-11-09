using BookingClinic.Infrastructure.Helpers.DoctorSorterHelper;

namespace BookingClinic.Infrastructure.Options
{
    public class DoctorSortingOptions
    {
        public Dictionary<string, IDoctorSorterStrategy> Strategies { get; set; }
    }
}
