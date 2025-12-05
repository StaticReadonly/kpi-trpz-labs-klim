using BookingClinic.Application.Data.Appointment;
using BookingClinic.Application.Data.Doctor;
using BookingClinic.Application.Interfaces.Helpers;
using BookingClinic.Application.Interfaces.Services;
using Mapster;
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
        private readonly IDoctorService _doctorService;
        private readonly IUserContextHelper _userContextHelper;
        private readonly IViewMessageHelper _viewMessageHelper;

        public AppointmentController(
            IAppointmentService appointmentService,
            IPaginationHelper<PatientAppointmentDto> patientPaginationHelper,
            IPaginationHelper<DoctorAppointmentDto> doctorAppointmentHelper,
            IDoctorService doctorService,
            IUserContextHelper userContextHelper,
            IViewMessageHelper viewMessageHelper)
        {
            _appointmentService = appointmentService;
            _patientPaginationHelper = patientPaginationHelper;
            _doctorAppointmentHelper = doctorAppointmentHelper;
            _doctorService = doctorService;
            _userContextHelper = userContextHelper;
            _viewMessageHelper = viewMessageHelper;
        }

        [HttpGet]
        [Authorize(AuthorizationPolicies.AuthorizedUserOnlyPolicy)]
        public IActionResult Index([FromQuery] int page)
        {
            if (_userContextHelper.IsDoctor)
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
                    _viewMessageHelper.SetErrors(res.Errors, TempData);
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
                    _viewMessageHelper.SetErrors(res.Errors, TempData);
                    return View("Patient", new List<PatientAppointmentDto>());
                }
            }
        }

        [HttpGet("finishedApp")]
        [Authorize(AuthorizationPolicies.DoctorOnlyPolicy)]
        public IActionResult FinishedAppointment([FromQuery] Guid patientId)
        {
            var docId = _userContextHelper.UserId!.Value;
            var res = _doctorService.GetDoctorData(docId);

            if (res.IsSuccess)
            {
                var data = res.Result.Adapt<AppointmentDoctorDataDto>();
                data.PatientId = patientId;
                return View("Finish", data);
            }
            else
            {
                _viewMessageHelper.SetErrors(res.Errors, TempData);
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
                _viewMessageHelper.SetSuccess("Appointment finished successfully", TempData);
                return RedirectToAction("FinishedAppointment", new {patientId = dto.PatientId});
            }
            else
            {
                _viewMessageHelper.SetErrors(res.Errors, TempData);
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        [Authorize(AuthorizationPolicies.AuthorizedUserOnlyPolicy)]
        public async Task<IActionResult> MakeAppointment(
            [FromForm] MakeAppointmentDto dto)
        {
            var res = await _appointmentService.CreateAppointment(dto);

            if (!res.IsSuccess)
            {
                _viewMessageHelper.SetErrors(res.Errors, TempData);
                return RedirectToAction("Profile", "Doctor", new { id = dto.DoctorId });
            }

            _viewMessageHelper.SetSuccess("Appointment created successfully", TempData);
            return RedirectToAction("Index", "Appointment");
        }

        [HttpPost("docApp")]
        [Authorize(AuthorizationPolicies.DoctorOnlyPolicy)]
        public async Task<IActionResult> MakeAppointmentDoctor(
            [FromForm] MakeAppointmentDocDto dto)
        {
            var res = await _appointmentService.CreateAppointmentDoctor(dto);

            if (res.IsSuccess)
            {
                _viewMessageHelper.SetSuccess("Appointment created successfully", TempData);
                return RedirectToAction("FinishedAppointment", new { dto.PatientId });
            }
            else
            {
                _viewMessageHelper.SetErrors(res.Errors, TempData);
                return RedirectToAction("FinishedAppointment", new { patientId = dto.PatientId });
            }
        }

        [HttpPost("cancel")]
        [Authorize(AuthorizationPolicies.PatientOnlyPolicy)]
        public async Task<IActionResult> CancelAppointment([FromQuery] Guid id)
        {
            var res = await _appointmentService.CancelAppointment(id);

            if (res.IsSuccess)
            {
                _viewMessageHelper.SetSuccess("Appointment canceled successfully", TempData);
                return RedirectToAction("Index");
            }
            else
            {
                _viewMessageHelper.SetErrors(res.Errors, TempData);
                return RedirectToAction("Index");
            }
        }
    }
}
