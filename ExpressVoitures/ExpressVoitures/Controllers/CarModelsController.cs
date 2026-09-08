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
        private const string unknownMake = "Marque inconnue";

        public CarModelsController(ICarModelService carModelService, ICarMakeService carMakeService) : base(carModelService)
        {
            _carMakeService = carMakeService;
        }

        // Endpoint pour AJAX : retourne les modèles d'une marque donnée
        [HttpGet]
        public IActionResult GetModelsByMake(int makeId)
        {
            var models = _service.GetViewModels()
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

            ViewData[dataExistingItems] = GetCarModelsWithMakes();
        }
        private List<CarModelViewModel> GetCarModelsWithMakes()
        {
            var collection = _service.GetViewModels();
            
            foreach (var item in collection)
            {
                var makeVm = _carMakeService.GetViewModel(item.MakeId);
                item.Make = makeVm ?? new CarMakeViewModel { Id = 0, Name = unknownMake };
            }

            return collection.OrderBy(m => m.Make?.Name).ThenBy(m => m.Name).ToList();
        }
    }
}
