using BookingClinic.Services.Helpers.DoctorsSortingHelper.DoctorSorterStrategies;

namespace BookingClinic.Services.Options
{
    public class DoctorSortingOptions
    {
        public Dictionary<string, IDoctorSorterStrategy> Strategies { get; set; }
    }
}
