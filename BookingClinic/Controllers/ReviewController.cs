using BookingClinic.Services.Review;
using Microsoft.AspNetCore.Mvc;

namespace BookingClinic.Controllers
{
    [Route("reviews")]
    public class ReviewController : Controller
    {
        private readonly IReviewService _reviewService;

        public ReviewController(
            IReviewService reviewService)
        {
            _reviewService = reviewService;
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
    }
}
