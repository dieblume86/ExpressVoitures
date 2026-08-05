using AutoMapper;
using ExpressVoitures.Models.Entities;
using ExpressVoitures.Models.ViewModels;

namespace ExpressVoitures.Models.Profiles
{
    public class CarModelProfile : Profile
    {
        public CarModelProfile()
        {
            CreateMap<CarModel, CarModelViewModel>();
            CreateMap<CarModelViewModel, CarModel>();
        }
    }
}
