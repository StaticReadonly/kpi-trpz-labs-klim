using BookingClinic.Application.Common;
using BookingClinic.Application.Data.Doctor;
using BookingClinic.Application.Interfaces.Services;
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
        private readonly ISearchDoctorFacade _searchDoctorFacade;

        public DoctorController(
            IDoctorService doctorService,
            IAppointmentService appointmentService,
            ISearchDoctorFacade searchDoctorFacade)
        {
            _doctorService = doctorService;
            _appointmentService = appointmentService;
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
                ViewData["Errors"] = JsonSerializer.Deserialize<List<ServiceError>>(TempData["Errors"]!.ToString()!);
            }

            if (TempData["ReviewErrors"] != null)
            {
                List<ValidationFailure> failures = 
                    JsonSerializer.Deserialize<List<ValidationFailure>>(TempData["ReviewErrors"]!.ToString()!);

                var valRes = new ValidationResult(failures);
                valRes.AddToModelState(ModelState);
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

        [HttpPost("docApp")]
        [Authorize(AuthorizationPolicies.DoctorOnlyPolicy)]
        public async Task<IActionResult> MakeAppointmentDoctor(
            [FromForm] MakeAppointmentDocDto dto)
        {
            var res = await _appointmentService.CreateAppointmentDoctor(dto);

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
