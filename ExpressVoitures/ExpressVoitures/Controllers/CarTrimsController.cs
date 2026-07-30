using ExpressVoitures.Models.Services.Interfaces;
using ExpressVoitures.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExpressVoitures.Controllers
{
    public class CarTrimsController : Controller
    {
        private readonly ICarTrimService _carTrimService;

        public CarTrimsController(ICarTrimService carTrimService)
        {
            _carTrimService = carTrimService;
        }

        [Authorize]
        [HttpPost]
        public IActionResult Create(CarTrimViewModel model)
        {
            _carTrimService.Add(model);
            return View();
        }

        [Authorize]
        [HttpPost]
        public IActionResult Delete(int id)
        {
            _carTrimService.Delete(id);
            return View();
        }
    }
}
