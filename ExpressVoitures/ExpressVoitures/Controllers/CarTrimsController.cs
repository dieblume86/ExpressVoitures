using ExpressVoitures.Models.Entities;
using ExpressVoitures.Models.Services;
using ExpressVoitures.Models.Services.Interfaces;
using ExpressVoitures.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
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

        // Endpoint pour AJAX : retourne les modèles d'une marque donnée
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
                item.CarModelViewModel = modelVm ?? new CarModelViewModel { Id = 0, Name = unknownModel };

                var makeVm = _carMakeService.GetViewModel(modelVm.MakeId);
                item.CarModelViewModel.CarMakeViewModel = makeVm ?? new CarMakeViewModel { Id = 0, Name = unknownMake };
            }

            return collection.OrderBy(m => m.CarModelViewModel?.CarMakeViewModel?.Name).ThenBy(m => m.CarModelViewModel?.Name).ThenBy(m => m.Name).ToList();
        }
    }
}
