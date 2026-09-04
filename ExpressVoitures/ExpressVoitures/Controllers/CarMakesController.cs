using ExpressVoitures.Models.Entities;
using ExpressVoitures.Models.Services.Interfaces;
using ExpressVoitures.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ExpressVoitures.Controllers
{
    public class CarMakesController : GenericEntityController<CarMake, CarMakeViewModel, ICarMakeService>
    {
        public CarMakesController(ICarMakeService carMakeService) : base(carMakeService)
        {
        }


        protected override void SetViewDatas()
        {
            ViewData[dataExistingItems] = _service.GetViewModels().OrderBy(m => m.Name);
        }
    }
}
