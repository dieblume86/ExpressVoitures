using ExpressVoitures.Models.Entities;
using ExpressVoitures.Models.Services.Interfaces;
using ExpressVoitures.Models.ViewModels;

namespace ExpressVoitures.Controllers
{
    public class CarsController : GenericEntityController<Car, CarViewModel, ICarService>
    {
        public CarsController(ICarService carService) : base(carService)
        {
        }

        //private readonly ICarService _carService;

        //public CarsController(ICarService carService)
        //{
        //    _carService = carService;
        //}


        //public virtual IActionResult Index()
        //{
        //    IEnumerable<CarViewModel> viewModels = _carService.GetViewModels();

        //    return View(viewModels);
        //}

        //[Authorize]
        //[HttpGet]
        //public IActionResult Create()
        //{
        //    return View(new CarViewModel());
        //}

        //[Authorize]
        //[HttpPost]
        //public IActionResult Create(CarViewModel product)
        //{
        //    return RedirectToAction("Index");
        //}
    }
}
