using BookingClinic.Application.Interfaces.Visitor;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookingClinic.Controllers
{
    [Route("manage")]
    [Authorize(AuthorizationPolicies.AdminOnlyPolicy)]
    public class AdminController : Controller
    {
        private readonly IUserExportService _userExportService;

        public AdminController(IUserExportService userExportService)
        {
            _userExportService = userExportService;
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
    }
}
