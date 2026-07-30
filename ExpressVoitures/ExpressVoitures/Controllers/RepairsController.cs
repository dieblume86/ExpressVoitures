using ExpressVoitures.Models.Services.Interfaces;
using ExpressVoitures.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExpressVoitures.Controllers
{
    public class RepairsController : Controller
    {
        private readonly IRepairService _repairService;

        public RepairsController(IRepairService repairService)
        {
            _repairService = repairService;
        }

        [Authorize]
        [HttpPost]
        public IActionResult Create(RepairViewModel model)
        {
            _repairService.Add(model);
            return View();
        }

        [Authorize]
        [HttpPost]
        public IActionResult Delete(int id)
        {
            _repairService.Delete(id);
            return View();
        }
    }
}
