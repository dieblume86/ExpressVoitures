using ExpressVoitures.Models.Entities;
using ExpressVoitures.Models.Services.Interfaces;
using ExpressVoitures.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;

namespace ExpressVoitures.Controllers
{
    public class CarMakesController : GenericEntityController<CarMake, CarMakeViewModel, ICarMakeService>
    {

        public CarMakesController(ICarMakeService carMakeService) : base(carMakeService)
        {
        }
    }
}
