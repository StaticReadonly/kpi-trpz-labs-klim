using BookingClinic.Services;
using BookingClinic.Services.Appointment;
using BookingClinic.Services.Clinic;
using BookingClinic.Services.Data.Appointment;
using BookingClinic.Services.Data.Doctor;
using BookingClinic.Services.Doctor;
using BookingClinic.Services.Helpers.PaginationHelper;
using BookingClinic.Services.Speciality;
using BookingClinic.Services.UserService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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

        public DoctorController(
            IUserService userService,
            IPaginationHelper<SearchDoctorResDto> paginationHelper,
            ISpecialityService specialityService,
            IClinicService clinicService,
            IDoctorService doctorService,
            IAppointmentService appointmentService)
        {
            _userService = userService;
            _paginationHelper = paginationHelper;
            _specialityService = specialityService;
            _clinicService = clinicService;
            _doctorService = doctorService;
            _appointmentService = appointmentService;
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

        [HttpGet("{id:guid}")]
        public IActionResult Profile([FromRoute] Guid id)
        {
            if (TempData["Errors"] != null)
            {
                ViewData["Errors"] = JsonSerializer.Deserialize<List<ServiceError>>(TempData["Errors"].ToString());
            }

            var res = _doctorService.GetDoctorData(id);

            if (res.IsSuccess)
            {
                return View(res.Result);
            }
            else
            {
                ViewData["Errors"] = res.Errors;
                return View();
            }
        }

        [HttpPost]
        [Authorize("PatientAppointment")]
        public async Task<IActionResult> MakeAppointment(
            [FromForm] MakeAppointmentDto dto)
        {
            var res = await _appointmentService.CreateAppointment(dto, User);

            if (!res.IsSuccess)
            {
                TempData["Errors"] = JsonSerializer.Serialize(res.Errors);
            }
            return RedirectToAction("Profile", new {id = dto.DoctorId});
        }
    }
}
