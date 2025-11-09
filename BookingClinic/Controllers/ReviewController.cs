//using BookingClinic.Services;
//using BookingClinic.Services.Data.Review;
//using BookingClinic.Services.Review;
using BookingClinic.Application.Interfaces.Services;
using BookingClinic.Application.Data.Review;
using BookingClinic.Application.Common;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace BookingClinic.Controllers
{
    [Route("reviews")]
    public class ReviewController : Controller
    {
        private readonly IReviewService _reviewService;
        private readonly IValidator<AddReviewDto> _addReviewValidator;

        public ReviewController(
            IReviewService reviewService, 
            IValidator<AddReviewDto> addReviewValidator)
        {
            _reviewService = reviewService;
            _addReviewValidator = addReviewValidator;
        }

        [HttpGet("{id:guid}")]
        public IActionResult Index([FromRoute] Guid id)
        {
            var res = _reviewService.GetDoctorReviews(id);

            if (res.IsSuccess)
            {
                return View(res.Result);
            }
            else
            {
                ViewData["Errors"] = res.Errors;
                return View();
            }
        }

        [HttpPost]
        [Authorize(AuthorizationPolicies.PatientOnlyPolicy)]
        public async Task<IActionResult> CreateReview([FromForm] AddReviewDto dto)
        {
            var validationRes = _addReviewValidator.Validate(dto);

            if (!validationRes.IsValid)
            {
                TempData["Errors"] = JsonSerializer.Serialize(
                    new List<ServiceError>() { ServiceError.InvalidReviewData()});
                return RedirectToAction("Profile", "Doctor", new { id = dto.DoctorId});
            }

            var res = await _reviewService.CreateReview(dto, User);

            if (res.IsSuccess)
            {
                return RedirectToAction("Index", new {id = dto.DoctorId});
            }
            else
            {
                TempData["Errors"] = JsonSerializer.Serialize(res.Errors);
                return RedirectToAction("Profile", "Doctor", new { id = dto.DoctorId });
            }
        }
    }
}
