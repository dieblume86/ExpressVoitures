using ExpressVoitures.Models.Entities;
using ExpressVoitures.Models.Services.Interfaces;
using ExpressVoitures.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExpressVoitures.Controllers
{
    public class CarsController : GenericEntityController<Car, CarViewModel, ICarService>
    {
        public CarsController(ICarService carService) : base(carService)
        {
        }
    }
}
