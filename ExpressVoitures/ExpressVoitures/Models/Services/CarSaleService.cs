using AutoMapper;
using ExpressVoitures.Models.Entities;
using ExpressVoitures.Models.Repositories.Interfaces;
using ExpressVoitures.Models.Services.Interfaces;
using ExpressVoitures.Models.ViewModels;
namespace ExpressVoitures.Models.Services
{
    public class CarSaleService : GenericEntityService<CarSale, CarSaleViewModel>, ICarSaleService
    {
        public CarSaleService(ICarSaleRepository carSaleRepository, IMapper mapper) : base(carSaleRepository, mapper)
        {
        }
    }
}
