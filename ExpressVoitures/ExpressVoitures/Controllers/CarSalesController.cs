using ExpressVoitures.Models.Entities;
using ExpressVoitures.Models.Services.Interfaces;
using ExpressVoitures.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExpressVoitures.Controllers
{
    public class CarSalesController : GenericEntityController<CarSale,CarSaleViewModel, ICarSaleService>
    {
        public CarSalesController(ICarSaleService carSaleService) : base(carSaleService)
        {
        }
    }
}
