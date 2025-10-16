using Microsoft.AspNetCore.Mvc;

namespace BookingClinic.Controllers
{
    [Route("error")]
    public class ErrorController : Controller
    {
        [Route("errPage")]
        public async Task<IActionResult> Index()
        {
            return View();
        }
    }
}
