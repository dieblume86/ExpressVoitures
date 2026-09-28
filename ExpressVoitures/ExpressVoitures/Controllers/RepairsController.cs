using ExpressVoitures.Models.Entities;
using ExpressVoitures.Models.Services.Interfaces;
using ExpressVoitures.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ExpressVoitures.Controllers
{
    public class RepairsController : GenericEntityController<Repair, RepairViewModel, IRepairService>
    {
        public RepairsController(IRepairService service) : base(service)
        {
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public override IActionResult Create(RepairViewModel model)
        {
            if (model == null)
            {
                TempData["Error"] = "Données de réparation manquantes.";
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Informations invalides pour la réparation.";
                return RedirectToAction("Edit", "Cars", new { id = model.CarId });
            }

            _service.Add(model);
            TempData["Success"] = "Réparation ajoutée.";
            return RedirectToAction("Edit", "Cars", new { id = model.CarId });
        }

        //[HttpGet]
        //[Authorize]
        //public override IActionResult Edit(int id)
        //{
        //    try
        //    {
        //        var vm = _service.GetViewModel(id);
        //        if (vm == null)
        //        {
        //            // Log possible here (ILogger) — pour l'instant on renvoie NotFound clair
        //            TempData["Error"] = $"Réparation introuvable (id={id}).";
        //            return RedirectToAction("Edit", "Cars"); // redirige vers liste/écran voitures si nécessaire
        //        }

        //        // si besoin remplir ViewData spécifiques (ex. SelectLists) : SetViewDatas();
        //        return View(vm);
        //    }
        //    catch (Exception ex)
        //    {
        //        // Pour debug rapide : stocker message et stack dans TempData (supprimer en prod)
        //        TempData["Error"] = "Erreur lors du chargement de la réparation : " + ex.Message;
        //        TempData["ErrorDetail"] = ex.ToString();
        //        return RedirectToAction("Edit", "Cars");
        //    }
        //}

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public override IActionResult Edit(RepairViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                SetViewDatas();
                return View(viewModel);
            }

            _service.Update(viewModel);

            TempData["Success"] = "La réparation a été mise à jour.";
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
                TempData["Success"] = "La réparation a été supprimée.";
            }
            catch (DbUpdateException)
            {
                // Erreur typique : contrainte FK (des modèles/voitures liées)
                TempData["Error"] = "Impossible de supprimer cette réparation : des enregistrements liés existent.";
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

            return RedirectToAction(nameof(Create));
        }
    }
}
