using BookingClinic.Services.Clinic;
using BookingClinic.Services.Data.Doctor;
using BookingClinic.Services.Helpers.PaginationHelper;
using BookingClinic.Services.Speciality;
using BookingClinic.Services.UserService;
using Microsoft.AspNetCore.Mvc;

namespace BookingClinic.Controllers
{
    [Route("doctors")]
    public class DoctorController : Controller
    {
        private readonly IUserService _userService;
        private readonly ISpecialityService _specialityService;
        private readonly IClinicService _clinicService;
        private readonly IPaginationHelper<SearchDoctorResDto> _paginationHelper;

        public DoctorController(
            IUserService userService,
            IPaginationHelper<SearchDoctorResDto> paginationHelper,
            ISpecialityService specialityService,
            IClinicService clinicService)
        {
            _userService = userService;
            _paginationHelper = paginationHelper;
            _specialityService = specialityService;
            _clinicService = clinicService;
        }

        [HttpGet]
        public IActionResult Index([FromQuery] SearchDoctorDto dto, [FromQuery] int page, [FromQuery] string? orderBy)
        {
            var res = _userService.SearchDoctors(dto);
            var specialities = _specialityService.GetSpecialityNames();
            var clinics = _clinicService.GetClinicNames();

            if (specialities.IsSuccess)
            {
                ViewData["Specialities"] = specialities.Result;
            }

            if (clinics.IsSuccess)
            {
                ViewData["Clinics"] = clinics.Result;
            }
            
            ViewData["Ordering"] = orderBy;
            ViewData["Page"] = page;

            if (res.IsSuccess)
            {
                var doctors = _paginationHelper.Paginate(res.Result, page, 5, out var pages);

                ViewData["Pages"] = pages;
                ViewData["Doctors"] = doctors;

                return View(dto);
            }
            else
            {
                ViewData["Page"] = 1;
                ViewData["Pages"] = new List<int> { 1 };
                ViewData["Errors"] = res.Errors;
                ViewData["Doctors"] = new List<SearchDoctorResDto>() { };

                return View(dto);
            }
        }
    }
}
