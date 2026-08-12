using ExpressVoitures.Models.Entities;
using ExpressVoitures.Models.Services.Interfaces;
using ExpressVoitures.Models.ViewModels;

namespace ExpressVoitures.Controllers
{
    public class CarMakesController : GenericEntityController<CarMake, CarMakeViewModel, ICarMakeService>
    {
        public CarMakesController(ICarMakeService carMakeService) : base(carMakeService)
        {
        }
    }
}
