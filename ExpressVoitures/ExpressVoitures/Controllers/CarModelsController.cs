using ExpressVoitures.Models.Entities;
using ExpressVoitures.Models.Services.Interfaces;
using ExpressVoitures.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExpressVoitures.Controllers
{
    public class CarModelsController : GenericEntityController<CarModel, CarModelViewModel, ICarModelService>
    {
        public CarModelsController(ICarModelService carModelService) : base(carModelService)
        {
        }
    }
}
