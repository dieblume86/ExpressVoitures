using AutoMapper;
using ExpressVoitures.Models.Entities;
using ExpressVoitures.Models.ViewModels;

namespace ExpressVoitures.Models.Profiles
{
    public class CarMakeProfile : Profile
    {
        public CarMakeProfile()
        {
            CreateMap<CarMake, CarMakeViewModel>();
            CreateMap<CarMakeViewModel, CarMake>();
        }
    }
}
