using BookingClinic.Services;
using BookingClinic.Services.Appointment;
using BookingClinic.Services.Clinic;
using BookingClinic.Services.Data.Appointment;
using BookingClinic.Services.Data.Doctor;
using BookingClinic.Services.Doctor;
using BookingClinic.Services.Helpers.DoctorsSortingHelper.DoctorSorter;
using BookingClinic.Services.Helpers.DoctorsSortingHelper.DoctorSorterStrategies;
using BookingClinic.Services.Helpers.PaginationHelper;
using BookingClinic.Services.Helpers.ReviewsHelper;
using BookingClinic.Services.Options;
using BookingClinic.Services.Speciality;
using BookingClinic.Services.UserService;
using FluentValidation.AspNetCore;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace BookingClinic.Controllers
{
    [Route("doctors")]
    public class DoctorController : Controller
    {
        private readonly IUserService _userService;
        private readonly ISpecialityService _specialityService;
        private readonly IClinicService _clinicService;
        private readonly IDoctorService _doctorService;
        private readonly IAppointmentService _appointmentService;
        private readonly IPaginationHelper<SearchDoctorResDto> _paginationHelper;
        private readonly IReviewsHelper _reviewsHelper;
        private readonly IDoctorSorter _doctorSorter;
        private readonly Dictionary<string, IDoctorSorterStrategy> _docSortingStrategies;

        public DoctorController(
            IUserService userService,
            IPaginationHelper<SearchDoctorResDto> paginationHelper,
            ISpecialityService specialityService,
            IClinicService clinicService,
            IDoctorService doctorService,
            IAppointmentService appointmentService,
            IReviewsHelper reviewsHelper,
            IDoctorSorter doctorSorter,
            IOptions<DoctorSortingOptions> docSortingStrategies)
        {
            _userService = userService;
            _paginationHelper = paginationHelper;
            _specialityService = specialityService;
            _clinicService = clinicService;
            _doctorService = doctorService;
            _appointmentService = appointmentService;
            _reviewsHelper = reviewsHelper;
            _doctorSorter = doctorSorter;
            _docSortingStrategies = docSortingStrategies.Value.Strategies;
        }

        [HttpGet]
        public async Task<IActionResult> Index([FromQuery] SearchDoctorDto dto, [FromQuery] int page)
        {
            ViewData["Sortings"] = _docSortingStrategies.Keys.ToList();

            var res = await _userService.SearchDoctors(dto);

            if (!res.IsSuccess)
            {
                ViewData["Page"] = 1;
                ViewData["Pages"] = new List<int> { 1 };
                ViewData["Errors"] = res.Errors;
                ViewData["Doctors"] = new List<SearchDoctorResDto>() { };

                return View(dto);
            }

            var specialities = _specialityService.GetSpecialityNames();
            var clinics = _clinicService.GetClinicNames();

            if (specialities.IsSuccess)
            {
                ViewData["Specialities"] = specialities.Result;
            }
            else
            {
                ViewData["Specialities"] = Array.Empty<string>();
            }

            if (clinics.IsSuccess)
            {
                ViewData["Clinics"] = clinics.Result;
            }
            else
            {
                ViewData["Clinics"] = Array.Empty<string>();
            }

            ViewData["Page"] = page;

            _doctorSorter.SetStrategy(dto.OrderBy);

            var orderDoctors = _doctorSorter.Sort(res.Result!);

            var doctors = _paginationHelper.Paginate(orderDoctors, page, 5, out var pages);

            ViewData["Pages"] = pages;
            ViewData["Doctors"] = doctors;

            return View(dto);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> Profile([FromRoute] Guid id)
        {
            if (TempData["Errors"] != null)
            {
                ViewData["Errors"] = JsonSerializer.Deserialize<List<ServiceError>>(TempData["Errors"].ToString());
            }

            if (TempData["ReviewErrors"] != null)
            {
                List<ValidationFailure> failures = 
                    JsonSerializer.Deserialize<List<ValidationFailure>>(TempData["ReviewErrors"].ToString());

                var valRes = new ValidationResult(failures);
                valRes.AddToModelState(ModelState);
            }

            var res = await _doctorService.GetDoctorData(id);

            if (res.IsSuccess)
            {
                if (User.IsInRole("Patient") || User.IsInRole("Admin"))
                {
                    res.Result!.CanWriteReview = _reviewsHelper.CanUserWriteReview(id, User);
                }

                return View(res.Result);
            }
            else
            {
                ViewData["Errors"] = res.Errors;
                return View();
            }
        }

        [HttpPost]
        [Authorize("AuthUser")]
        public async Task<IActionResult> MakeAppointment(
            [FromForm] MakeAppointmentDto dto)
        {
            var res = await _appointmentService.CreateAppointment(dto, User);

            if (!res.IsSuccess)
            {
                TempData["Errors"] = JsonSerializer.Serialize(res.Errors);
                return RedirectToAction("Profile", new { id = dto.DoctorId });
            }
            return RedirectToAction("Index", "User");
        }

        [HttpPost("docApp")]
        [Authorize("Doctors")]
        public async Task<IActionResult> MakeAppointmentDoctor(
            [FromForm] MakeAppointmentDocDto dto)
        {
            var res = await _appointmentService.CreateAppointmentDoctor(dto, User);

            if (res.IsSuccess)
            {
                return RedirectToAction("FinishedAppointment", "Appointment", new { dto.PatientId });
            }
            else
            {
                TempData["Errors"] = JsonSerializer.Serialize(res.Errors);
                return RedirectToAction("Index", "Appointment");
            }
        }
    }
}
