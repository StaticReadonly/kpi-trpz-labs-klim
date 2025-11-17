using BookingClinic.Application.Interfaces.Services;
using BookingClinic.Application.Interfaces.Helpers;
using BookingClinic.Application.Data.Appointment;
using BookingClinic.Application.Common;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text.Json;

namespace BookingClinic.Controllers
{
    [Route("appointment")]
    public class AppointmentController : Controller
    {
        private readonly IAppointmentService _appointmentService;
        private readonly IPaginationHelper<PatientAppointmentDto> _patientPaginationHelper;
        private readonly IPaginationHelper<DoctorAppointmentDto> _doctorAppointmentHelper;
        private readonly IDoctorService _doctorService;

        public AppointmentController(
            IAppointmentService appointmentService,
            IPaginationHelper<PatientAppointmentDto> patientPaginationHelper,
            IPaginationHelper<DoctorAppointmentDto> doctorAppointmentHelper,
            IDoctorService doctorService)
        {
            _appointmentService = appointmentService;
            _patientPaginationHelper = patientPaginationHelper;
            _doctorAppointmentHelper = doctorAppointmentHelper;
            _doctorService = doctorService;
        }

        [HttpGet]
        [Authorize(AuthorizationPolicies.AuthorizedUserOnlyPolicy)]
        public IActionResult Index([FromQuery] int page)
        {
            if (TempData["Errors"] != null)
            {
                ViewData["Errors"] = JsonSerializer.Deserialize<List<ServiceError>>(TempData["Errors"].ToString());
            }

            if (User.IsInRole("Doctor"))
            {
                var res = _appointmentService.GetDoctorAppointments();

                if (res.IsSuccess)
                {
                    var paginated = _doctorAppointmentHelper.Paginate(res.Result, page, 5, out var pages);
                    ViewData["Page"] = page;
                    ViewData["Pages"] = pages;
                    return View("Doctor", paginated);
                }
                else
                {
                    ViewData["Page"] = 1;
                    ViewData["Pages"] = new List<int> { 1 };
                    ViewData["Errors"] = res.Errors;
                    return View("Doctor", new List<DoctorAppointmentDto>());
                }
            }
            else
            {
                var res = _appointmentService.GetPatientAppointments();

                if (res.IsSuccess)
                {
                    var paginated = _patientPaginationHelper.Paginate(res.Result, page, 5, out var pages);
                    ViewData["Page"] = page;
                    ViewData["Pages"] = pages;
                    return View("Patient", paginated);
                }
                else
                {
                    ViewData["Page"] = 1;
                    ViewData["Pages"] = new List<int> { 1 };
                    ViewData["Errors"] = res.Errors;
                    return View("Patient", new List<PatientAppointmentDto>());
                }
            }
        }

        [HttpGet("finishedApp")]
        [Authorize(AuthorizationPolicies.DoctorOnlyPolicy)]
        public IActionResult FinishedAppointment([FromQuery] Guid patientId)
        {
            var docId = User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier).Value;

            var res = _doctorService.GetDoctorData(Guid.Parse(docId));

            if (res.IsSuccess)
            {
                var data = res.Result.Adapt<AppointmentDoctorDataDto>();
                data.PatientId = patientId;
                return View("Finish", data);
            }
            else
            {
                ViewData["Errors"] = res.Errors;
                return RedirectToAction("FinishedAppointment", new {patientId});
            }
        }

        [HttpPost("finish")]
        [Authorize(AuthorizationPolicies.DoctorOnlyPolicy)]
        public async Task<IActionResult> FinishAppointment([FromForm] FinishAppointmentDto dto)
        {
            var res = await _appointmentService.FinishAppointment(dto);

            if (res.IsSuccess)
            {
                return RedirectToAction("FinishedAppointment", new {patientId = dto.PatientId});
            }
            else
            {
                ViewData["Errors"] = res.Errors;
                return RedirectToAction("Index");
            }
        }

        [HttpPost("cancel")]
        [Authorize(AuthorizationPolicies.PatientOnlyPolicy)]
        public async Task<IActionResult> CancelAppointment([FromQuery] Guid id)
        {
            var res = await _appointmentService.CancelAppointment(id);

            if (res.IsSuccess)
            {
                return RedirectToAction("Index");
            }
            else
            {
                ViewData["Errors"] = res.Errors;
                return RedirectToAction("Index");
            }
        }
    }
}
