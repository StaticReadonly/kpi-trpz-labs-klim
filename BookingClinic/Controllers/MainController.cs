using Microsoft.AspNetCore.Mvc;

namespace BookingClinic.Controllers
{
    [Route("/")]
    public class MainController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
    }
}
