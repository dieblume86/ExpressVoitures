using ExpressVoitures.Models.Entities;
using ExpressVoitures.Models.Services.Interfaces;
using ExpressVoitures.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ExpressVoitures.Controllers
{
    public class CarModelsController : GenericEntityController<CarModel, CarModelViewModel, ICarModelService>
    {
        private readonly ICarMakeService _carMakeService;

        public CarModelsController(ICarModelService carModelService, ICarMakeService carMakeService) : base(carModelService)
        {
            _carMakeService = carMakeService;
        }

        [HttpGet]
        public override IActionResult Create()
        {
            ViewData["Makes"] = new SelectList(_carMakeService.GetViewModels(), "Id", "Name");
            return base.Create();
        }

        [HttpPost]
        public override IActionResult Create(CarModelViewModel viewModel)
        {
            // Toujours fournir la liste avant d'appeler la logique du base (pour le cas où base renverrait View(viewModel))
            ViewData["Makes"] = new SelectList(_carMakeService.GetViewModels(), "Id", "Name", viewModel.MakeId);
            return base.Create(viewModel);
        }
    }
}
