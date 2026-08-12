using ExpressVoitures.Models.Entities;
using ExpressVoitures.Models.Services.Interfaces;
using ExpressVoitures.Models.ViewModels;

namespace ExpressVoitures.Controllers
{
    public class CarModelsController : GenericEntityController<CarModel, CarModelViewModel, ICarModelService>
    {
        public CarModelsController(ICarModelService carModelService) : base(carModelService)
        {
        }
    }
}
