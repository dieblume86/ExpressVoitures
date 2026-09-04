using ExpressVoitures.Models.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ExpressVoitures.Controllers
{

    //[Authorize]
    public class GenericEntityController<TEntity, TViewModel, TService> : Controller
        where TEntity : class
        where TViewModel : class, new()
        where TService : IGenericEntityService<TEntity, TViewModel>
    {
        protected readonly TService _service;

        protected const string dataExistingItems = "ExistingItems";

        public GenericEntityController(TService service)
        {
            _service = service;
        }

        public virtual IActionResult Index()
        {
            IEnumerable<TViewModel> viewModels = _service.GetViewModels();

            return View(viewModels);
        }

        [Authorize]
        [HttpGet]
        public virtual IActionResult Create()
        {
            SetViewDatas();

            return View(new TViewModel());
        }

        [Authorize]
        [HttpPost]
        public virtual IActionResult Create(TViewModel viewModel)
        {
            //TODO trim name already exists for the same model and return an error message if so

            IEnumerable<string> modelErrors = _service.CheckModelErrors(viewModel);

            foreach (string error in modelErrors)
            {
                ModelState.AddModelError("", error);
            }

            if (ModelState.IsValid)
            {
                _service.Add(viewModel);

                TempData["Success"] = "Success.";
                return RedirectToAction(nameof(Create));
            }
            else
            {
                TempData["Error"] = "Une erreur est survenue.";
                return RedirectToAction(nameof(Create));
            }
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public virtual IActionResult Delete(int id)
        {
            try
            {
                _service.Delete(id);
                TempData["Success"] = "La marque a été supprimée.";
            }
            catch (DbUpdateException)
            {
                // Erreur typique : contrainte FK (des modèles/voitures liées)
                TempData["Error"] = "Impossible de supprimer cette marque : des enregistrements liés existent.";
            }
            catch (Exception)
            {
                TempData["Error"] = "Une erreur est survenue lors de la suppression.";
            }

            return RedirectToAction(nameof(Create));
        }


        protected virtual void SetViewDatas()
        {
            ViewData[dataExistingItems] = _service.GetViewModels();
        }
    }
}
