using BookingClinic.Application.Data.User;
using BookingClinic.Application.Interfaces.Helpers;
using BookingClinic.Application.Interfaces.Services;
using FluentValidation;
using FluentValidation.AspNetCore;
using Mapster;
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
        private readonly IValidator<UserPageDataDto> _userDataValidator;
        private readonly IViewMessageHelper _viewMessageHelper;

        public UserController(
            IUserService userService,
            IValidator<LoginUserDto> loginValidator,
            IValidator<RegisterUserDto> registerValidator,
            IValidator<UserPageDataDto> userDataValidator,
            IViewMessageHelper viewMessageHelper)
        {
            _userService = userService;
            _loginValidator = loginValidator;
            _registerValidator = registerValidator;
            _userDataValidator = userDataValidator;
            _viewMessageHelper = viewMessageHelper;
        }

        [HttpGet]
        [Authorize(AuthorizationPolicies.AuthorizedUserOnlyPolicy)]
        public async Task<IActionResult> Index()
        {
            var userData = _userService.GetUserData();

            if (userData.IsSuccess)
            {
                return View(userData.Result.Adapt<UserPageDataUpdateDto>());
            }
            else
            {
                await Logout();
                return LoginPage("/user");
            }
        }

        [HttpGet("login")]
        public IActionResult LoginPage([FromQuery] string? returnUrl)
        {
            return View(new LoginUserDto() {ReturnUrl = returnUrl });
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

                if (dto.ReturnUrl != null)
                {
                    return Redirect(dto.ReturnUrl);
                }

                return RedirectToAction("Index");
            }
            else
            {
                _viewMessageHelper.SetErrors(res.Errors, TempData);
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
                _viewMessageHelper.SetErrors(res.Errors, TempData);
                return View("RegisterPage", dto);
            }
        }


        [HttpPost]
        [Authorize(AuthorizationPolicies.AuthorizedUserOnlyPolicy)]
        public async Task <IActionResult> UpdateUser([FromForm] UserPageDataUpdateDto newData)
        {
            var validationRes = _userDataValidator.Validate(newData);

            if (!validationRes.IsValid)
            {
                validationRes.AddToModelState(ModelState);
                return RedirectToAction("Index");
            }

            var res = await _userService.UpdateUser(newData);

            if (res.IsSuccess)
            {
                return RedirectToAction("Index");
            }
            else
            {
                _viewMessageHelper.SetErrors(res.Errors, TempData);
                return RedirectToAction("Index");
            }
        }

        [HttpPost("ppc")]
        [Authorize(AuthorizationPolicies.AuthorizedUserOnlyPolicy)]
        public async Task<IActionResult> ProfilePicture([FromForm] IFormFile image)
        {
            using var stream = image.OpenReadStream();
            var newImageModel = new UserPictureDto(image.FileName, stream);
            var res = await _userService.UpdateUserPhoto(newImageModel);

            if (res.IsSuccess)
            {
                return RedirectToAction("Index");
            }
            else
            {
                _viewMessageHelper.SetErrors(res.Errors, TempData);
                return RedirectToAction("Index");
            }
        }

        [HttpPost("logout")]
        [Authorize(AuthorizationPolicies.AuthorizedUserOnlyPolicy)]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Index", "Main");
        }
    }
}
