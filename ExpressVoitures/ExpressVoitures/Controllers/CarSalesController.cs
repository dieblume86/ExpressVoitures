using ExpressVoitures.Models.Services.Interfaces;
using ExpressVoitures.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExpressVoitures.Controllers
{
    public class CarSalesController : Controller
    {
        private readonly ICarSaleService _carSaleService;

        public CarSalesController(ICarSaleService carSaleService)
        {
            _carSaleService = carSaleService;
        }

        [Authorize]
        [HttpPost]
        public IActionResult Create(CarSaleViewModel model)
        {
            _carSaleService.Add(model);
            return View();
        }

        [Authorize]
        [HttpPost]
        public IActionResult Delete(int id)
        {
            _carSaleService.Delete(id);
            return View();
        }
    }
}
