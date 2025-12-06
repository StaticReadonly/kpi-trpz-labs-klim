using BookingClinic.Application.Common;
using BookingClinic.Application.Data.Doctor;
using BookingClinic.Application.Helpers;
using BookingClinic.Application.Interfaces.Helpers;
using BookingClinic.Application.Interfaces.Services;
using BookingClinic.Application.Options;
using Microsoft.Extensions.Options;

namespace BookingClinic.Application.Services
{
    public class SearchDoctorFacade : ISearchDoctorFacade
    {
        private readonly IDoctorSorter _doctorSorter;
        private readonly IPaginationHelper<SearchDoctorResDto> _paginationHelper;
        private readonly ISpecialityService _specialityService;
        private readonly IClinicService _clinicService;
        private readonly IUserService _userService;
        private readonly Dictionary<string, IDoctorSorterStrategy> _docSortingStrategies;

        public SearchDoctorFacade(
            IDoctorSorter doctorSorter,
            IPaginationHelper<SearchDoctorResDto> paginationHelper,
            ISpecialityService specialityService,
            IClinicService clinicService,
            IUserService userService,
            IOptions<DoctorSortingOptions> docSortingStrategies)
        {
            _doctorSorter = doctorSorter;
            _paginationHelper = paginationHelper;
            _specialityService = specialityService;
            _clinicService = clinicService;
            _userService = userService;
            _docSortingStrategies = docSortingStrategies.Value.Strategies;
        }

        public SearchDoctorIndexResult SearchForDoctors(SearchDoctorDto dto, int page)
        {
            var doctorsRes = _userService.SearchDoctors(dto);
            var specialitiesRes = _specialityService.GetSpecialityNames();
            var clinicsRes = _clinicService.GetClinicNames();

            List<ServiceError> errors = new();
            var doctors = doctorsRes.Result!;

            if (!doctorsRes.IsSuccess)
            {
                errors.AddRange(doctorsRes.Errors);
                doctors = Enumerable.Empty<SearchDoctorResDto>();
            }

            _doctorSorter.SetStrategy(dto.OrderBy);
            doctors = _doctorSorter.Sort(doctors);
            doctors = _paginationHelper.Paginate(doctors, page, 5, out var pages);

            if (!specialitiesRes.IsSuccess)
            {
                errors.AddRange(specialitiesRes.Errors);
            }

            if (!clinicsRes.IsSuccess)
            {
                errors.AddRange(clinicsRes.Errors);
            }

            var res = new SearchDoctorIndexResult()
            {
                IsSuccess = errors.Count == 0,
                Doctors = doctors,
                Clinics = clinicsRes.IsSuccess ? clinicsRes.Result! : Enumerable.Empty<string>(),
                Specialities = specialitiesRes.IsSuccess ? specialitiesRes.Result! : Enumerable.Empty<string>(),
                Errors = errors,
                Page = (!doctorsRes.IsSuccess || page == 0) ? 1 : page,
                Sortings = _docSortingStrategies.Keys.ToList(),
                Pages = pages,
            };

            return res;
        }
    }
}
