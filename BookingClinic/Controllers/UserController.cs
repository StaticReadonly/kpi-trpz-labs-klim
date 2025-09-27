using BookingClinic.Services.UserService;
using Microsoft.AspNetCore.Mvc;

namespace BookingClinic.Controllers
{
    [Route("user")]
    public class UserController : Controller
    {
        private readonly IUserService _userService;

        public UserController(
            IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
    }
}
