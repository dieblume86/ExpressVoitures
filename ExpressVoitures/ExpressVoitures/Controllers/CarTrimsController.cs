using ExpressVoitures.Models.Entities;
using ExpressVoitures.Models.Services.Interfaces;
using ExpressVoitures.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ExpressVoitures.Controllers
{
    public class CarTrimsController : GenericEntityController<CarTrim, CarTrimViewModel, ICarTrimService>
    {
        private readonly ICarMakeService _carMakeService;
        private const string unknownMake = "Marque inconnue";

        private readonly ICarModelService _carModelService;
        private const string unknownModel = "Modèle inconnu";

        public CarTrimsController(ICarTrimService carTrimService, ICarMakeService carMakeService, ICarModelService carModelService) : base(carTrimService)
        {
            _carMakeService = carMakeService;
            _carModelService = carModelService;
        }

        [HttpGet]
        public override IActionResult Edit(int id)
        {
            var vm = _service.GetViewModel(id);
            if (vm == null) 
                return NotFound();

            var modelVm = _carModelService.GetViewModel(vm.ModelId);
            var selectedMakeId = modelVm?.MakeId ?? 0;

            var makes = _carMakeService.GetViewModels().OrderBy(m => m.Name).ToList();
            ViewData["Makes"] = new SelectList(makes, "Id", "Name", selectedMakeId);

            var models = _carModelService.GetViewModels()
                .Where(m => m.MakeId == selectedMakeId)
                .OrderBy(m => m.Name)
                .ToList();
            ViewData["Models"] = new SelectList(models, "Id", "Name", vm.ModelId);

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public override IActionResult Edit(CarTrimViewModel viewModel)
        {
            var selectedModelId = viewModel?.ModelId ?? 0;
            var modelVm = _carModelService.GetViewModel(selectedModelId);
            var selectedMakeId = modelVm?.MakeId ?? 0;

            var makes = _carMakeService.GetViewModels().OrderBy(m => m.Name).ToList();
            ViewData["Makes"] = new SelectList(makes, "Id", "Name", selectedMakeId);

            var models = _carModelService.GetViewModels()
                .Where(m => m.MakeId == selectedMakeId)
                .OrderBy(m => m.Name)
                .ToList();
            ViewData["Models"] = new SelectList(models, "Id", "Name", selectedModelId);

            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            _service.Update(viewModel);
            TempData["Success"] = "Finition mise à jour.";
            return RedirectToAction(nameof(Create));
        }

        // Endpoint for AJAX
        [HttpGet]
        public IActionResult GetTrimsByModel(int modelId)
        {
            var trims = _service.GetViewModels()
                .Where(t => t.ModelId == modelId)
                .OrderBy(t => t.Name)
                .Select(t => new { id = t.Id, name = t.Name })
                .ToList();

            return Json(trims);
        }


        protected override void SetViewDatas()
        {
            var makes = _carMakeService.GetViewModels().OrderBy(m => m.Name);
            ViewData["Makes"] = new SelectList(makes, "Id", "Name");

            var models = _carModelService.GetViewModels().OrderBy(m => m.Name);
            ViewData["Models"] = new SelectList(Enumerable.Empty<object>(), "Id", "Name");

            ViewData[dataExistingItems] = GetCarTrimsWithParents();
        }
        private List<CarTrimViewModel> GetCarTrimsWithParents()
        {
            var collection = _service.GetViewModels();

            foreach (var item in collection)
            {
                var modelVm = _carModelService.GetViewModel(item.ModelId);
                item.Model = modelVm ?? new CarModelViewModel { Id = 0, Name = unknownModel };

                var makeVm = _carMakeService.GetViewModel(modelVm.MakeId);
                item.Model.Make = makeVm ?? new CarMakeViewModel { Id = 0, Name = unknownMake };
            }

            return collection.OrderBy(m => m.Model?.Make?.Name).ThenBy(m => m.Model?.Name).ThenBy(m => m.Name).ToList();
        }
    }
}
