using ExpressVoitures.Helpers;
using ExpressVoitures.Models.Entities;
using ExpressVoitures.Models.Services.Interfaces;
using ExpressVoitures.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ExpressVoitures.Controllers
{
    public class CarTrimsController : GenericEntityController<CarTrim, CarTrimViewModel, ICarTrimService>
    {
        public CarTrimsController(ICarTrimService carTrimService) : base(carTrimService)
        {

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
    }
}
