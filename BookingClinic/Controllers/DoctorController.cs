using BookingClinic.Application.Data.Doctor;
using BookingClinic.Application.Interfaces.Helpers;
using BookingClinic.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace BookingClinic.Controllers
{
    [Route("doctors")]
    public class DoctorController : Controller
    {
        private readonly IDoctorService _doctorService;
        private readonly ISearchDoctorFacade _searchDoctorFacade;
        private readonly IViewMessageHelper _viewMessageHelper;

        public DoctorController(
            IDoctorService doctorService,
            ISearchDoctorFacade searchDoctorFacade,
            IViewMessageHelper viewMessageHelper)
        {
            _doctorService = doctorService;
            _searchDoctorFacade = searchDoctorFacade;
            _viewMessageHelper = viewMessageHelper;
        }

        [HttpGet]
        public IActionResult Index([FromQuery] SearchDoctorDto dto, [FromQuery] int page)
        {
            var res = _searchDoctorFacade.SearchForDoctors(dto, page);

            ViewData["Specialities"] = res.Specialities;
            ViewData["Clinics"] = res.Clinics;
            ViewData["Sortings"] = res.Sortings;
            ViewData["Page"] = res.Page;
            ViewData["Pages"] = res.Pages;
            ViewData["Doctors"] = res.Doctors;
            _viewMessageHelper.SetErrors(res.Errors, TempData);

            return View(dto);
        }

        [HttpGet("{id:guid}")]
        public IActionResult Profile([FromRoute] Guid id)
        {
            var res = _doctorService.GetDoctorData(id);

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
    }
}
