using AutoMapper;
using ExpressVoitures.Models.Entities;
using ExpressVoitures.Models.Repositories.Interfaces;
using ExpressVoitures.Models.Services.Interfaces;
using ExpressVoitures.Models.ViewModels;

namespace ExpressVoitures.Models.Services
{
    public class CarService : GenericEntityService<Car, CarViewModel>, ICarService
    {
        public CarService(ICarRepository carRepository, IMapper mapper) : base(carRepository, mapper)
        {
        }
    }
}
