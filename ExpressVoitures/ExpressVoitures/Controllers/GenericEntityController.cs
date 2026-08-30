using ExpressVoitures.Models.Services.Interfaces;
using ExpressVoitures.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
            ViewData[dataExistingItems] = _service.GetViewModels();
            return View(new TViewModel());
        }

        [Authorize]
        [HttpPost]
        public virtual IActionResult Create(TViewModel viewModel)
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
                return RedirectToAction(nameof(Create));
            }
            else
            {
                ViewData[dataExistingItems] = _service.GetViewModels();
                return View(viewModel);
            }
        }

        [Authorize]
        [HttpPost]
        public virtual IActionResult Delete(int id)
        {
            //_service.Delete(id);
            //return View();
            ////return RedirectToAction("Admin");
            _service.Delete(id);

            ViewData[dataExistingItems] = _service.GetViewModels();
            return RedirectToAction(nameof(Create));
        }
    }
}
