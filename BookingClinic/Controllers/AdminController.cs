using BookingClinic.Application.Common;
using BookingClinic.Application.Data.Admin;
using BookingClinic.Application.Data.User;
using BookingClinic.Application.Interfaces.Services;
using BookingClinic.Application.Interfaces.Visitor;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace BookingClinic.Controllers
{
    [Route("manage")]
    [Authorize(AuthorizationPolicies.AdminOnlyPolicy)]
    public class AdminController : Controller
    {
        private readonly IUserExportService _userExportService;
        private readonly IAdminService _adminService;

        public AdminController(IUserExportService userExportService, IAdminService adminService)
        {
            _userExportService = userExportService;
            _adminService = adminService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult GeneratePdfUsers()
        {
            _userExportService.ExportUsersToPdf();
            return RedirectToAction("Index");
        }


        [HttpGet("users")]
        public IActionResult Users()
        {
            if (TempData["Errors"] != null)
            {
                ViewData["Errors"] = JsonSerializer.Deserialize<List<ServiceError>>(TempData["Errors"].ToString());
            }

            var users = _adminService.GetAllUsers();
            return View(users);
        }

        [HttpGet("users/create")]
        public IActionResult CreateUser()
        {
            ViewData["Clinics"] = _adminService.GetAllClinics();
            ViewData["Specialities"] = _adminService.GetAllSpecialities();
            return View(new UserAdminDto());
        }

        [HttpPost("users/create")]
        public async Task<IActionResult> CreateUserPost([FromForm] UserAdminDto dto)
        {
            ViewData["Clinics"] = _adminService.GetAllClinics();
            ViewData["Specialities"] = _adminService.GetAllSpecialities();
            if (!ModelState.IsValid)
            {
                return View("CreateUser", dto);
            }

            var res = await _adminService.RegisterUser(dto);

            if (!res.IsSuccess)
            {
                //TempData["Errors"] = JsonSerializer.Serialize(res.Errors);
                ViewData["Errors"] = res.Errors;
                return View("CreateUser", dto);
            }

            return RedirectToAction("Users");
        }

        [HttpGet("users/edit/{id:guid}")]
        public async Task<IActionResult> EditUser([FromRoute] Guid id)
        {
            ViewData["Clinics"] = _adminService.GetAllClinics();
            ViewData["Specialities"] = _adminService.GetAllSpecialities();
            var res = await _adminService.GetUserById(id);

            if (!res.IsSuccess)
            {
                TempData["Errors"] = JsonSerializer.Serialize(res.Errors);
                return RedirectToAction("Users");
            }

            return View(res.Result);
        }

        [HttpPost("users/edit")]
        public async Task<IActionResult> EditUserPost([FromForm] UserAdminDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Password) && string.IsNullOrWhiteSpace(dto.ConfirmPassword))
            {
                ModelState.Remove(nameof(dto.Password));
                ModelState.Remove(nameof(dto.ConfirmPassword));
            }

            ViewData["Clinics"] = _adminService.GetAllClinics();
            ViewData["Specialities"] = _adminService.GetAllSpecialities();
            if (!ModelState.IsValid)
            {
                return View("EditUser", dto);
            } 

            var res = await _adminService.UpdateUser(dto);

            if (!res.IsSuccess)
            {
                TempData["Errors"] = JsonSerializer.Serialize(res.Errors);
                return RedirectToAction("EditUser", new { id = dto.Id });
            }

            return RedirectToAction("Users");
        }

        [HttpPost("users/delete/{id:guid}")]
        public async Task<IActionResult> DeleteUser([FromRoute] Guid id)
        {
            var res = await _adminService.DeleteUser(id);

            if (!res.IsSuccess)
            {
                TempData["Errors"] = JsonSerializer.Serialize(res.Errors);
            }

            return RedirectToAction("Users");
        }

        [HttpGet("clinics")]
        public IActionResult Clinics()
        {
            if (TempData["Errors"] != null)
            {
                ViewData["Errors"] = JsonSerializer.Deserialize<List<ServiceError>>(TempData["Errors"].ToString());
            }

            var clinics = _adminService.GetAllClinics();
            return View(clinics);
        }

        [HttpGet("clinics/create")]
        public IActionResult CreateClinic()
        {
            return View(new ClinicAdminDto());
        }

        [HttpPost("clinics/create")]
        public async Task<IActionResult> CreateClinicPost([FromForm] ClinicAdminDto dto)
        {
            if (!ModelState.IsValid)
                return View("CreateClinic", dto);

            var res = await _adminService.CreateClinic(dto);

            if (!res.IsSuccess)
            {
                //TempData["Errors"] = JsonSerializer.Serialize(res.Errors);
                ViewData["Errors"] = res.Errors;
                return View("CreateClinic", dto);
            }

            return RedirectToAction("Clinics");
        }

        [HttpGet("clinics/edit/{id:guid}")]
        public IActionResult EditClinic([FromRoute] Guid id)
        {
            var clinic = _adminService.GetAllClinics().FirstOrDefault(c => c.Id == id);
            if (clinic == null)
            {
                TempData["Errors"] = JsonSerializer.Serialize(new List<ServiceError> { ServiceError.UnexpectedError() });
                return RedirectToAction("Clinics");
            }

            return View(clinic);
        }

        [HttpPost("clinics/edit")]
        public async Task<IActionResult> EditClinicPost([FromForm] ClinicAdminDto dto)
        {
            if (!ModelState.IsValid)
                return View("EditClinic", dto);

            var res = await _adminService.UpdateClinic(dto);

            if (!res.IsSuccess)
            {
                TempData["Errors"] = JsonSerializer.Serialize(res.Errors);
                return RedirectToAction("EditClinic", new { id = dto.Id });
            }

            return RedirectToAction("Clinics");
        }

        [HttpPost("clinics/delete/{id:guid}")]
        public async Task<IActionResult> DeleteClinic([FromRoute] Guid id)
        {
            var res = await _adminService.DeleteClinic(id);

            if (!res.IsSuccess)
            {
                TempData["Errors"] = JsonSerializer.Serialize(res.Errors);
            }

            return RedirectToAction("Clinics");
        }

        [HttpGet("specialities")]
        public IActionResult Specialities()
        {
            if (TempData["Errors"] != null)
            {
                ViewData["Errors"] = JsonSerializer.Deserialize<List<ServiceError>>(TempData["Errors"].ToString());
            }

            var specs = _adminService.GetAllSpecialities();
            return View(specs);
        }

        [HttpGet("specialities/create")]
        public IActionResult CreateSpeciality()
        {
            return View(new SpecialityAdminDto());
        }

        [HttpPost("specialities/create")]
        public async Task<IActionResult> CreateSpecialityPost([FromForm] SpecialityAdminDto dto)
        {
            if (!ModelState.IsValid)
                return View("CreateSpeciality", dto);

            var res = await _adminService.CreateSpeciality(dto);

            if (!res.IsSuccess)
            {
                //TempData["Errors"] = JsonSerializer.Serialize(res.Errors);
                ViewData["Errors"] = res.Errors;
                return View("CreateSpeciality", dto);
            }

            return RedirectToAction("Specialities");
        }

        [HttpGet("specialities/edit/{id:guid}")]
        public IActionResult EditSpeciality([FromRoute] Guid id)
        {
            var spec = _adminService.GetAllSpecialities().FirstOrDefault(s => s.Id == id);
            if (spec == null)
            {
                TempData["Errors"] = JsonSerializer.Serialize(new List<ServiceError> { ServiceError.UnexpectedError() });
                return RedirectToAction("Specialities");
            }

            return View(spec);
        }

        [HttpPost("specialities/edit")]
        public async Task<IActionResult> EditSpecialityPost([FromForm] SpecialityAdminDto dto)
        {
            if (!ModelState.IsValid)
                return View("EditSpeciality", dto);

            var res = await _adminService.UpdateSpeciality(dto);

            if (!res.IsSuccess)
            {
                TempData["Errors"] = JsonSerializer.Serialize(res.Errors);
                return RedirectToAction("EditSpeciality", new { id = dto.Id });
            }

            return RedirectToAction("Specialities");
        }

        [HttpPost("specialities/delete/{id:guid}")]
        public async Task<IActionResult> DeleteSpeciality([FromRoute] Guid id)
        {
            var res = await _adminService.DeleteSpeciality(id);

            if (!res.IsSuccess)
            {
                TempData["Errors"] = JsonSerializer.Serialize(res.Errors);
            }

            return RedirectToAction("Specialities");
        }
    }
}
