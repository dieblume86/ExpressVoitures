using ExpressVoitures.Models.Services.Interfaces;
using ExpressVoitures.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExpressVoitures.Controllers
{
    public class CarMakesController : Controller
    {
        private readonly ICarMakeService _carMakeService;

        public CarMakesController(ICarMakeService carMakeService)
        {
            _carMakeService = carMakeService;
        }

        //public IActionResult Index()
        //{
        //    IEnumerable<CarMakeViewModel> makes = _carMakeService.GetAllCarMakesViewModel();

        //    return View(makes);
        //}

        [Authorize]
        [HttpPost]
        public IActionResult Create(CarMakeViewModel make)
        {
            _carMakeService.Add(make);
            return View();

            //IEnumerable<string> modelErrors = _carMakeService.CheckModelErrors(make);

            //foreach (string error in modelErrors)
            //{
            //    ModelState.AddModelError("", error);
            //}

            //if (ModelState.IsValid)
            //{
            //    _carMakeService.Add(make);
            //    return RedirectToAction("Admin");
            //}
            //else
            //{
            //    return View(make);
            //}
        }

        [Authorize]
        [HttpPost]
        public IActionResult Delete(int id)
        {
            _carMakeService.Remove(id);
            return View();
            //return RedirectToAction("Admin");
        }
    }
}
