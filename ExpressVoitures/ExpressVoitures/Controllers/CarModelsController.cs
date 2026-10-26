using ExpressVoitures.Models.Entities;
using ExpressVoitures.Models.Services.Interfaces;
using ExpressVoitures.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace ExpressVoitures.Controllers
{
    public class CarModelsController : GenericEntityController<CarModel, CarModelViewModel, ICarModelService>
    {
        public CarModelsController(ICarModelService carModelService) : base(carModelService)
        {
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
     }
}
