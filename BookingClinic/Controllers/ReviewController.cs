using BookingClinic.Application.Data.Review;
using BookingClinic.Application.Interfaces.Helpers;
using BookingClinic.Application.Interfaces.Services;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookingClinic.Controllers
{
    [Route("reviews")]
    public class ReviewController : Controller
    {
        private readonly IReviewService _reviewService;
        private readonly IValidator<AddReviewDto> _addReviewValidator;
        private readonly IViewMessageHelper _viewMessageHelper;

        public ReviewController(
            IReviewService reviewService, 
            IValidator<AddReviewDto> addReviewValidator,
            IViewMessageHelper viewMessageHelper)
        {
            _reviewService = reviewService;
            _addReviewValidator = addReviewValidator;
            _viewMessageHelper = viewMessageHelper;
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
                _viewMessageHelper.SetErrors(res.Errors, TempData);
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
                return RedirectToAction("Profile", "Doctor", new { id = dto.DoctorId});
            }

            var res = await _reviewService.CreateReview(dto);

            if (res.IsSuccess)
            {
                _viewMessageHelper.SetSuccess("Review added successfully", TempData);
                return RedirectToAction("Index", new {id = dto.DoctorId});
            }
            else
            {
                _viewMessageHelper.SetErrors(res.Errors, TempData);
                return RedirectToAction("Profile", "Doctor", new { id = dto.DoctorId });
            }
        }

        [HttpPost("reviewDelete")]
        [Authorize(AuthorizationPolicies.AuthorizedUserOnlyPolicy)]
        public async Task<IActionResult> DeleteReview([FromForm] DeleteReviewDto dto)
        {
            var res = await _reviewService.DeleteReview(dto);

            if (res.IsSuccess)
            {
                _viewMessageHelper.SetSuccess("Review deleted successfully", TempData);
                return RedirectToAction("Index", new {id = dto.DoctorId});
            }
            else
            {
                _viewMessageHelper.SetErrors(res.Errors, TempData);
                return RedirectToAction("Index", new {id = dto.DoctorId});
            }
        }
    }
}
