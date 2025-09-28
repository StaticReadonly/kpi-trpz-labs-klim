using BookingClinic.Services.Data.User;
using BookingClinic.Services.UserService;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookingClinic.Controllers
{
    [Route("user")]
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        private readonly IValidator<LoginUserDto> _loginValidator;

        public UserController(
            IUserService userService, 
            IValidator<LoginUserDto> loginValidator)
        {
            _userService = userService;
            _loginValidator = loginValidator;
        }

        [HttpGet]
        [Authorize("AuthUser")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("login")]
        public IActionResult LoginPage()
        {
            return View();
        }

        [HttpPost("loginPost")]
        public async Task<IActionResult> LoginPost([FromForm] LoginUserDto dto)
        {
            var validationResult = _loginValidator.Validate(dto);

            if (!validationResult.IsValid)
            {
                validationResult.AddToModelState(ModelState);
                return View("LoginPage", dto);
            }

            var res = _userService.LoginUser(dto);

            if (res.IsSuccess)
            {
                await HttpContext.SignInAsync(res.Result);
                return RedirectToAction("Index");
            }
            else
            {
                ViewData["Errors"] = res.Errors;
                return View("LoginPage", dto);
            }
        }

        [HttpGet("register")]
        public IActionResult RegisterPage()
        {
            return View();
        }

        [HttpPost("ppc")]
        [Authorize("AuthUser")]
        public IActionResult ProfilePicture([FromForm] IFormFile image)
        {
            return RedirectToAction("Index");
        }

        [HttpPost("logout")]
        [Authorize("AuthUser")]
        public IActionResult Logout()
        {
            return RedirectToAction("Index", "Main");
        }
    }
}
