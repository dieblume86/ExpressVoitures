using ExpressVoitures.Models.Entities;
using ExpressVoitures.Models.Services.Interfaces;
using ExpressVoitures.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ExpressVoitures.Controllers
{
    public class CarSalesController : GenericEntityController<CarSale,CarSaleViewModel, ICarSaleService>
    {
        private readonly ICarService _carService;
        public CarSalesController(ICarSaleService carSaleService, ICarService carService) : base(carSaleService)
        {
            _carService = carService;
        }

        // Override la Create GET du contrôleur générique pour éviter l'ambiguïté.
        // On lit optionnellement carId depuis la query string et on pré-remplit le ViewModel.
        [Authorize]
        [HttpGet]
        public override IActionResult Create()
        {
            var vm = new CarSaleViewModel();

            // Récupère carId depuis la query string si présent : /CarSales/Create?carId=123
            var carIdStr = HttpContext.Request.Query["carId"].FirstOrDefault();
            if (int.TryParse(carIdStr, out var carId))
            {
                vm.CarId = carId;
            }

            return View(vm);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public override IActionResult Create(CarSaleViewModel viewModel)
        {
            IEnumerable<string> modelErrors = _service.CheckModelErrors(viewModel);

            foreach (string error in modelErrors)
            {
                ModelState.AddModelError("", error);
            }

            if (ModelState.IsValid)
            {
                _service.Add(viewModel);
                TempData["Success"] = "Success.";
                return RedirectToAction("Edit", "Cars", new { id = viewModel.CarId });
            }
            else
            {
                TempData["Error"] = "Informations invalides.";
                return RedirectToAction("Edit", "Cars", new { id = viewModel.CarId });
            }
        }
    }
}
