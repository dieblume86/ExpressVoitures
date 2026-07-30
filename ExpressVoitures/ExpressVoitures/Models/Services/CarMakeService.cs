using AutoMapper;
using ExpressVoitures.Models.Entities;
using ExpressVoitures.Models.Repositories.Interfaces;
using ExpressVoitures.Models.Services.Interfaces;
using ExpressVoitures.Models.ViewModels;

namespace ExpressVoitures.Models.Services
{
    public class CarMakeService : GenericEntityService<CarMake, CarMakeViewModel>, ICarMakeService
    {
        public CarMakeService(ICarMakeRepository carMakeRepository, IMapper mapper) : base(carMakeRepository, mapper)
        {
        }
    }
}
