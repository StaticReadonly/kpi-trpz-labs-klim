using BookingClinic.Services.Appointment;
using BookingClinic.Services.Data.Appointment;
using BookingClinic.Services.Helpers.PaginationHelper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookingClinic.Controllers
{
    [Route("appointment")]
    public class AppointmentController : Controller
    {
        private readonly IAppointmentService _appointmentService;
        private readonly IPaginationHelper<PatientAppointmentDto> _patientPaginationHelper;
        private readonly IPaginationHelper<DoctorAppointmentDto> _doctorAppointmentHelper;

        public AppointmentController(
            IAppointmentService appointmentService, 
            IPaginationHelper<PatientAppointmentDto> patientPaginationHelper, 
            IPaginationHelper<DoctorAppointmentDto> doctorAppointmentHelper)
        {
            _appointmentService = appointmentService;
            _patientPaginationHelper = patientPaginationHelper;
            _doctorAppointmentHelper = doctorAppointmentHelper;
        }

        [HttpGet]
        [Authorize("AuthUser")]
        public IActionResult Index([FromQuery] int page)
        {
            if (User.IsInRole("Doctor"))
            {
                var res = _appointmentService.GetDoctorAppointments(User);

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
                var res = _appointmentService.GetPatientAppointments(User);

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
        public IActionResult FinishedAppointment()
        {
            return View();
        }

        [HttpPost("finish")]
        [Authorize("Doctors")]
        public async Task<IActionResult> FinishAppointment([FromForm] FinishAppointmentDto dto)
        {
            var res = await _appointmentService.FinishAppointment(dto);

            if (res.IsSuccess)
            {
                return RedirectToAction("FinishedAppointment");
            }
            else
            {
                ViewData["Errors"] = res.Errors;
                return RedirectToAction("Index");
            }
        }

        [HttpPost("cancel")]
        [Authorize("PatientAppointment")]
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
