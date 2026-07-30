using ExpressVoitures.Models.Services.Interfaces;
using ExpressVoitures.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExpressVoitures.Controllers
{
    public class CarModelsController : Controller
    {
        private readonly ICarModelService _carModelService;

        public CarModelsController(ICarModelService carModelService)
        {
            _carModelService = carModelService;
        }

        [Authorize]
        [HttpPost]
        public IActionResult Create(CarModelViewModel model)
        {
            _carModelService.Add(model);
            return View();
        }

        [Authorize]
        [HttpPost]
        public IActionResult Delete(int id)
        {
            _carModelService.Delete(id);
            return View();
        }
    }
}
