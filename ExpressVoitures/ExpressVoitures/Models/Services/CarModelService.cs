using AutoMapper;
using ExpressVoitures.Models.Entities;
using ExpressVoitures.Models.Repositories.Interfaces;
using ExpressVoitures.Models.Services.Interfaces;
using ExpressVoitures.Models.ViewModels;

namespace ExpressVoitures.Models.Services
{
    public class CarModelService : GenericEntityService<CarModel, CarModelViewModel>, ICarModelService
    {
        public CarModelService(ICarModelRepository carModelRepository, IMapper mapper) : base(carModelRepository, mapper)
        {
        }
    }
}
