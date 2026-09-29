using ExpressVoitures.Models.Entities;
using ExpressVoitures.Models.Services.Interfaces;
using ExpressVoitures.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ExpressVoitures.Controllers
{
    public class CarSalesController : GenericEntityController<CarSale, CarSaleViewModel, ICarSaleService>
    {
        private readonly IRepairService _repairService;
        public CarSalesController(ICarSaleService carSaleService, IRepairService repairService) : base(carSaleService)
        {
            _repairService = repairService;
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

            //var repairs = _repairService.GetViewModels().Where(r => r.CarId == viewModel.CarId).ToList();

            //if (repairs.Count > 0)
            //{
            //    foreach (var repair in repairs)
            //    {
            //        viewModel.SalePrice += repair.RepairCost;
            //    }
            //}

            //viewModel.SalePrice += viewModel.PurchasePrice + 500f;

            UpdateSalePrice(viewModel);  

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

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public override IActionResult Edit(CarSaleViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                SetViewDatas();
                return View(viewModel);
            }

            UpdateSalePrice(viewModel);

            _service.Update(viewModel);

            TempData["Success"] = "La fiche de vente a été mise à jour.";
            return RedirectToAction("Edit", "Cars", new { id = viewModel.CarId });
        }


        private void UpdateSalePrice(CarSaleViewModel viewModel)
        {
            var salePrice = 0f;

            var repairs = _repairService.GetViewModels().Where(r => r.CarId == viewModel.CarId).ToList();
            if (repairs.Count > 0)
            {
                foreach (var repair in repairs)
                {
                    salePrice += repair.RepairCost;
                }
            }
            salePrice += viewModel.PurchasePrice + 500f;

            viewModel.SalePrice = salePrice;
        }
    }
}
