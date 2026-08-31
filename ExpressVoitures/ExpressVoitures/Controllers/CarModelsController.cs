using ExpressVoitures.Models.Entities;
using ExpressVoitures.Models.Services.Interfaces;
using ExpressVoitures.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ExpressVoitures.Controllers
{
    public class CarModelsController : GenericEntityController<CarModel, CarModelViewModel, ICarModelService>
    {
        private readonly ICarMakeService _carMakeService;
        private const string unknownMake= "Marque inconnue";

        public CarModelsController(ICarModelService carModelService, ICarMakeService carMakeService) : base(carModelService)
        {
            _carMakeService = carMakeService;
        }

        [Authorize]
        [HttpGet]
        public override IActionResult Create()
        {
            var collection = _service.GetViewModels();

            foreach (var item in collection)
            {
                var makeVm = _carMakeService.GetViewModel(item.MakeId);
                item.CarMakeViewModel = makeVm ?? new CarMakeViewModel { Id = 0, Name = unknownMake };
            }

            ViewData[dataExistingItems] = collection.OrderBy(m=>m.CarMakeViewModel?.Name);

            var makes = _carMakeService.GetViewModels()
                .OrderBy(m => m.Name)
                ;

            ViewData["Makes"] = new SelectList(makes, "Id", "Name");

            return View(new CarModelViewModel());
        }

        [Authorize]
        [HttpPost]
        public override IActionResult Create(CarModelViewModel viewModel)
        {
            var makes = _carMakeService.GetViewModels()
                .OrderBy(m => m.Name)
                ;

            ViewData["Makes"] = new SelectList(makes, "Id", "Name", viewModel?.MakeId);

            var collection = _service.GetViewModels();
            foreach (var item in collection)
            {
                var makeVm = _carMakeService.GetViewModel(item.MakeId);
                item.CarMakeViewModel = makeVm ?? new CarMakeViewModel { Id = 0, Name = unknownMake };
            }

            ViewData[dataExistingItems] = collection.OrderBy(m => m.CarMakeViewModel?.Name);

            //TODO model name already exists for the same make and return an error message if so

            return base.Create(viewModel);
        }
    }
}
