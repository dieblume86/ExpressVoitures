using ExpressVoitures.Models.Entities;
using ExpressVoitures.Models.Services.Interfaces;
using ExpressVoitures.Models.ViewModels;

namespace ExpressVoitures.Controllers
{
    public class CarTrimsController : GenericEntityController<CarTrim, CarTrimViewModel,  ICarTrimService>
    {
        public CarTrimsController(ICarTrimService carTrimService) : base(carTrimService)
        {
        }
    }
}
