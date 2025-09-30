﻿using BookingClinic.Services;
using BookingClinic.Services.Data.User;
using BookingClinic.Services.UserService;
using FluentValidation;
using FluentValidation.AspNetCore;
using Mapster;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace BookingClinic.Controllers
{
    [Route("user")]
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        private readonly IValidator<LoginUserDto> _loginValidator;
        private readonly IValidator<RegisterUserDto> _registerValidator;
        private readonly IValidator<UserPageDataDto> _userDataValidator;

        public UserController(
            IUserService userService,
            IValidator<LoginUserDto> loginValidator,
            IValidator<RegisterUserDto> registerValidator,
            IValidator<UserPageDataDto> userDataValidator)
        {
            _userService = userService;
            _loginValidator = loginValidator;
            _registerValidator = registerValidator;
            _userDataValidator = userDataValidator;
        }

        [HttpGet]
        [Authorize("AuthUser")]
        public async Task<IActionResult> Index()
        {
            if (TempData["Errors"] != null)
            {
                ViewData["Errors"] = JsonSerializer.Deserialize<List<ServiceError>>(TempData["Errors"].ToString());
            }

            var userData = _userService.GetUserData(User);

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


        [HttpPost]
        [Authorize("AuthUser")]
        public async Task <IActionResult> UpdateUser([FromForm] UserPageDataUpdateDto newData)
        {
            var validationRes = _userDataValidator.Validate(newData);

            if (!validationRes.IsValid)
            {
                validationRes.AddToModelState(ModelState);
                return RedirectToAction("Index");
            }

            var res = await _userService.UpdateUser(newData, User);

            if (res.IsSuccess)
            {
                return RedirectToAction("Index");
            }
            else
            {
                TempData["Errors"] = JsonSerializer.Serialize(res.Errors);
                return RedirectToAction("Index");
            }
        }

        [HttpPost("ppc")]
        [Authorize("AuthUser")]
        public async Task<IActionResult> ProfilePicture([FromForm] IFormFile image)
        {
            var res = await _userService.UpdateUserPhoto(image, User);

            if (res.IsSuccess)
            {
                return RedirectToAction("Index");
            }
            else
            {
                ViewData["Errors"] = res.Errors;
                return RedirectToAction("Index");
            }
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
