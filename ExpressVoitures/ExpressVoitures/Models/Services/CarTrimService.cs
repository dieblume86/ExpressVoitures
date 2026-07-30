using AutoMapper;
using ExpressVoitures.Models.Entities;
using ExpressVoitures.Models.Repositories.Interfaces;
using ExpressVoitures.Models.Services.Interfaces;
using ExpressVoitures.Models.ViewModels;

namespace ExpressVoitures.Models.Services
{
    public class CarTrimService : GenericEntityService<CarTrim, CarTrimViewModel>, ICarTrimService
    {
        public CarTrimService(ICarTrimRepository carTrimRepository, IMapper mapper) : base(carTrimRepository, mapper)
        {
        }
    }
}
