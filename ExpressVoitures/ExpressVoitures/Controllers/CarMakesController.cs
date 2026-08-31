using ExpressVoitures.Models.Entities;
using ExpressVoitures.Models.Services;
using ExpressVoitures.Models.Services.Interfaces;
using ExpressVoitures.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ExpressVoitures.Controllers
{
    public class CarMakesController : GenericEntityController<CarMake, CarMakeViewModel, ICarMakeService>
    {
        public CarMakesController(ICarMakeService carMakeService) : base(carMakeService)
        {
        }

        [Authorize]
        [HttpGet]
        public override IActionResult Create()
        {
            SetViewDatas();

            return View(new CarMakeViewModel());
        }

        [Authorize]
        [HttpPost]
        public override IActionResult Create(CarMakeViewModel viewModel)
        {
            SetViewDatas();

            //TODO model name already exists for the same make and return an error message if so

            return base.Create(viewModel);
        }



        private void SetViewDatas()
        {
            ViewData[dataExistingItems] = _service.GetViewModels().OrderBy(m => m.Name);
        }
    }
}
