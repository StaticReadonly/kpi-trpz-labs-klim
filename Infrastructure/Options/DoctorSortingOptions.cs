using BookingClinic.Application.Helpers;

namespace BookingClinic.Infrastructure.Options
{
    public class DoctorSortingOptions
    {
        public Dictionary<string, IDoctorSorterStrategy> Strategies { get; set; }
    }
}
