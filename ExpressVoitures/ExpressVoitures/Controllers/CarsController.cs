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

        public override IActionResult Index()
        {
            return View(GetCars());
        }

        [HttpGet]
        public IActionResult Details(int id)
        {
            return View("Details", GetCar(id));
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

                return RedirectToAction(nameof(Details), viewModel.Id);
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

            ViewData[dataExistingItems] = GetCars().OrderBy(m => m.Trim?.Model?.Make?.Name)
                .ThenBy(m => m.Trim?.Model?.Name)
                .ThenBy(m => m.Trim?.Name)
                .ToList();
        }
        private List<CarViewModel> GetCars()
        {
            var cars = _service.GetViewModels().ToList();

            var makes = _carMakeService.GetViewModels().ToDictionary(m => m.Id);
            var models = _carModelService.GetViewModels().ToDictionary(m => m.Id);
            var trims = _carTrimService.GetViewModels().ToDictionary(t => t.Id);

            foreach (var car in cars)
            {
                makes.TryGetValue(car.MakeId, out var makeVm);
                models.TryGetValue(car.ModelId, out var modelVm);
                trims.TryGetValue(car.TrimId, out var trimVm);

                car.Make = makeVm;
                car.Model = modelVm;
                car.Trim = trimVm;
            }

            return cars;
        }
        private CarViewModel GetCar(int id)
        {
            var vm = _service.GetViewModel(id);

            if (vm == null)
                return new CarViewModel();

            vm.Trim = _carTrimService.GetViewModel(vm.TrimId);
            vm.Model = _carModelService.GetViewModel(vm.ModelId);
            vm.Make = _carMakeService.GetViewModel(vm.MakeId);

            return vm;
        }
    }
}
