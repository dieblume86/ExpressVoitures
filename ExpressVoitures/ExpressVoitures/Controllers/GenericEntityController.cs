using ExpressVoitures.Models.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExpressVoitures.Controllers
{

    [Authorize]
    public class GenericEntityController<TEntity, TViewModel, TService> : Controller
        where TEntity : class
        where TViewModel : class
        where TService : IGenericEntityService<TEntity, TViewModel>
    {
        protected readonly TService _service;

        public GenericEntityController(TService service)
        {
            _service = service;
        }

        //public virtual IActionResult Index()
        //{
        //    IEnumerable<CarMakeViewModel> makes = _carMakeService.GetAllCarMakesViewModel();

        //    return View(makes);
        //}

        [Authorize]
        [HttpPost]
        public virtual IActionResult Create(TViewModel viewModel)
        {
            _service.Add(viewModel);
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
        public virtual IActionResult Delete(int id)
        {
            _service.Delete(id);
            return View();
            //return RedirectToAction("Admin");
        }
    }
}
