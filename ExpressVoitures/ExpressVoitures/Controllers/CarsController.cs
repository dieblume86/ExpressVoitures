using ExpressVoitures.Models.Entities;
using ExpressVoitures.Models.Services.Interfaces;
using ExpressVoitures.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ExpressVoitures.Controllers
{
    public class CarsController : GenericEntityController<Car, CarViewModel, ICarService>
    {
        private readonly ICarMakeService _carMakeService;
        private const string unknownMake = "Marque inconnue";

        private readonly ICarModelService _carModelService;
        private const string unknownModel = "Modèle inconnu";

        private readonly ICarTrimService _carTrimService;
        private const string unknownTrim = "Finition inconnue";

        public CarsController(ICarService carService, ICarMakeService carMakeService, ICarModelService carModelService, ICarTrimService carTrimService) : base(carService)
        {
            _carMakeService = carMakeService;
            _carModelService = carModelService;
            _carTrimService = carTrimService;
        }

        [HttpGet]
        public virtual IActionResult SingleCar(CarViewModel viewModel)
        {
            return View(viewModel);
        }

        // Endpoint for AJAX
        [HttpGet]
        public IActionResult GetModelsByMake(int makeId)
        {
            var models = _carModelService.GetViewModels()
                .Where(m => m.MakeId == makeId)
                .OrderBy(m => m.Name)
                .Select(m => new { id = m.Id, name = m.Name })
                .ToList();

            return Json(models);
        }
        
        // Endpoint for AJAX
        [HttpGet]
        public IActionResult GetTrimsByModel(int modelId)
        {
            var trims = _carTrimService.GetViewModels()
                .Where(t => t.ModelId == modelId)
                .OrderBy(t => t.Name)
                .Select(t => new { id = t.Id, name = t.Name })
                .ToList();

            return Json(trims);
        }

        [Authorize]
        [HttpPost]
        public override IActionResult Create(CarViewModel viewModel)
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

                return RedirectToAction(nameof(SingleCar), viewModel);
            }
            else
            {
                SetViewDatas();
                return View(viewModel);
            }
        }

        protected override void SetViewDatas()
        {
            var makes = _carMakeService.GetViewModels().OrderBy(m => m.Name);
            ViewData["Makes"] = new SelectList(makes, "Id", "Name");

            var models = _carModelService.GetViewModels().OrderBy(m => m.Name);
            ViewData["Models"] = new SelectList(Enumerable.Empty<object>(), "Id", "Name");

            var trims = _carTrimService.GetViewModels().OrderBy(m => m.Name);
            ViewData["Trims"] = new SelectList(Enumerable.Empty<object>(), "Id", "Name");

            ViewData[dataExistingItems] = GetCarTrimsWithParents();
        }
        private List<CarViewModel> GetCarTrimsWithParents()
        {
            var collection = _service.GetViewModels();

            foreach (var item in collection)
            {
                var trimVm = _carTrimService.GetViewModel(item.TrimId);
                item.CarTrimViewModel = trimVm ?? new CarTrimViewModel { Id = 0, Name = unknownTrim };

                var modelVm = _carModelService.GetViewModel(trimVm.ModelId);
                item.CarTrimViewModel.CarModelViewModel = modelVm ?? new CarModelViewModel { Id = 0, Name = unknownModel };

                var makeVm = _carMakeService.GetViewModel(modelVm.MakeId);
                item.CarTrimViewModel.CarModelViewModel.CarMakeViewModel = makeVm ?? new CarMakeViewModel { Id = 0, Name = unknownMake };
            }

            return collection
                .OrderBy(m => m.CarTrimViewModel?.CarModelViewModel?.CarMakeViewModel?.Name)
                .ThenBy(m => m.CarTrimViewModel?.CarModelViewModel?.Name)
                .ThenBy(m => m.CarTrimViewModel?.Name)
                .ToList();
        }
    }
}
