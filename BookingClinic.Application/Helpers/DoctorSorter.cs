using BookingClinic.Application.Data.Doctor;
using BookingClinic.Application.Helpers.DoctorSorterHelper;
using BookingClinic.Application.Interfaces.Helpers;
using BookingClinic.Application.Options;
using Microsoft.Extensions.Options;

namespace BookingClinic.Application.Helpers
{
    public class DoctorSorter : IDoctorSorter
    {
        private readonly Dictionary<string, IDoctorSorterStrategy> _strategiesDict;

        private IDoctorSorterStrategy _strategy;

        public DoctorSorter(IOptions<DoctorSortingOptions> options)
        {
            _strategiesDict = options.Value.Strategies;
            _strategy = new NoStrategy();
        }

        public void SetStrategy(string? strategy)
        {
            if (!string.IsNullOrWhiteSpace(strategy) && _strategiesDict.ContainsKey(strategy))
            {
                _strategy = _strategiesDict[strategy];
            }
            else
            {
                _strategy = new NoStrategy();
            }
        }

        public IEnumerable<SearchDoctorResDto> Sort(IEnumerable<SearchDoctorResDto> items)
        {
            return _strategy.Sort(items);
        }
    }
}
