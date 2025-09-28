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
        private readonly IValidator<RegisterUserDto> _registerValidator;

        public UserController(
            IUserService userService,
            IValidator<LoginUserDto> loginValidator,
            IValidator<RegisterUserDto> registerValidator)
        {
            _userService = userService;
            _loginValidator = loginValidator;
            _registerValidator = registerValidator;
        }

        [HttpGet]
        [Authorize("AuthUser")]
        public async Task<IActionResult> Index()
        {
            var userData = _userService.GetUserData(User);

            if (userData.IsSuccess)
            {
                return View(userData.Result);
            }
            else
            {
                await Logout();
                return LoginPage();
            }
        }

        [HttpPost]
        [Authorize("AuthUser")]
        public IActionResult UpdateUser([FromForm] RegisterUserDto newData)
        {
            return RedirectToAction("Index");
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

        [HttpPost("register")]
        public async Task<IActionResult> RegisterPost([FromForm] RegisterUserDto dto)
        {
            var validationResult = _registerValidator.Validate(dto);

            if (dto.Password != dto.ConfirmPassword)
            {
                validationResult.Errors.Add(new("ConfirmPassword", "Passwords must match"));
            }

            if (!validationResult.IsValid)
            {
                validationResult.AddToModelState(ModelState);
                return View("RegisterPage", dto);
            }

            var res = await _userService.RegisterUser(dto);

            if (res.IsSuccess)
            {
                await HttpContext.SignInAsync(res.Result);
                return RedirectToAction("Index");
            }
            else
            {
                ViewData["Errors"] = res.Errors;
                return View("RegisterPage", dto);
            }
        }


        [HttpPost("ppc")]
        [Authorize("AuthUser")]
        public IActionResult ProfilePicture([FromForm] IFormFile image)
        {
            return RedirectToAction("Index");
        }

        [HttpPost("logout")]
        [Authorize("AuthUser")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Index", "Main");
        }
    }
}
