using BookingClinic.Application.Data.Doctor;

namespace BookingClinic.Application.Common
{
    public class SearchDoctorIndexResult
    {
        public bool IsSuccess { get; init; }
        public ICollection<ServiceError> Errors { get; init; }
        public IEnumerable<SearchDoctorResDto> Doctors { get; init; }
        public List<int> Pages { get; init; } = new();
        public int Page { get; init; }
        public IEnumerable<string> Specialities { get; init; }
        public IEnumerable<string> Clinics { get; init; }
        public IEnumerable<string> Sortings { get; init; }
    }
}
