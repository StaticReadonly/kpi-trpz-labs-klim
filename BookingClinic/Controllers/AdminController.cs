using BookingClinic.Services.Visitor;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookingClinic.Controllers
{
    [Route("manage")]
    [Authorize("Admin")]
    public class AdminController : Controller
    {
        private readonly UserExportService _userExportService;

        public AdminController(UserExportService userExportService)
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
