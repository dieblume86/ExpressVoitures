using ExpressVoitures.Models.Entities;
using ExpressVoitures.Models.Services.Interfaces;
using ExpressVoitures.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace ExpressVoitures.Controllers
{
    public class CarSalesController : GenericEntityController<CarSale, CarSaleViewModel, ICarSaleService>
    {
        private readonly IRepairService _repairService;
        public CarSalesController(ICarSaleService carSaleService, IRepairService repairService) : base(carSaleService)
        {
            _repairService = repairService;
        }

        [Authorize]
        [HttpGet("CarSales/Create/{carId}")]
        public IActionResult Create(int carId)
        {
            var vm = new CarSaleViewModel();
            vm.CarId = carId;

            return View(vm);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public override IActionResult Create(CarSaleViewModel viewModel)
        {
            IEnumerable<string> modelErrors = _service.CheckModelErrors(viewModel);

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

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public override IActionResult Delete(int id)
        {
            // Récupère l'id de la voiture avant suppression pour pouvoir rediriger correctement
            var carId = 0;
            try
            {
                var vm = _service.GetViewModel(id);
                if (vm != null)
                {
                    carId = vm.CarId;
                }

                _service.Delete(id);
                TempData["Success"] = "La fiche de vente a été supprimée.";
            }
            catch (DbUpdateException)
            {
                // Erreur typique : contrainte FK (des modèles/voitures liées)
                TempData["Error"] = "Impossible de supprimer cette fiche de vente : des enregistrements liés existent.";
            }
            catch (Exception)
            {
                TempData["Error"] = "Une erreur est survenue lors de la suppression.";
            }

            // Si carId vaut 0, on redirige vers la page Create des voitures (comportement existant précédemment)
            if (carId > 0)
            {
                return RedirectToAction("Edit", "Cars", new { id = carId });
            }

            return RedirectToAction("Index", "Cars");
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
