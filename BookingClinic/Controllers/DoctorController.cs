using BookingClinic.Services;
using BookingClinic.Services.Appointment;
using BookingClinic.Services.Data.Appointment;
using BookingClinic.Services.Data.Doctor;
using BookingClinic.Services.Doctor;
using BookingClinic.Services.Doctor.Facade;
using BookingClinic.Services.Helpers.ReviewsHelper;
using FluentValidation.AspNetCore;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace BookingClinic.Controllers
{
    [Route("doctors")]
    public class DoctorController : Controller
    {
        private readonly IDoctorService _doctorService;
        private readonly IAppointmentService _appointmentService;
        private readonly IReviewsHelper _reviewsHelper;
        private readonly ISearchDoctorFacade _searchDoctorFacade;

        public DoctorController(
            IDoctorService doctorService,
            IAppointmentService appointmentService,
            IReviewsHelper reviewsHelper,
            ISearchDoctorFacade searchDoctorFacade)
        {
            _doctorService = doctorService;
            _appointmentService = appointmentService;
            _reviewsHelper = reviewsHelper;
            _searchDoctorFacade = searchDoctorFacade;
        }

        [HttpGet]
        public IActionResult Index([FromQuery] SearchDoctorDto dto, [FromQuery] int page)
        {
            var res = _searchDoctorFacade.SearchForDoctors(dto, page);

            ViewData["Specialities"] = res.Specialities;
            ViewData["Clinics"] = res.Clinics;
            ViewData["Sortings"] = res.Sortings;
            ViewData["Page"] = res.Page;
            ViewData["Pages"] = res.Pages;
            ViewData["Doctors"] = res.Doctors;
            ViewData["Errors"] = res.Errors;

            return View(dto);
        }

        [HttpGet("{id:guid}")]
        public IActionResult Profile([FromRoute] Guid id)
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

            var res = _doctorService.GetDoctorData(id);

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
        [Authorize(AuthorizationPolicies.AuthorizedUserOnlyPolicy)]
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
        [Authorize(AuthorizationPolicies.DoctorOnlyPolicy)]
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
