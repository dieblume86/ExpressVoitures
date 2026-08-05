using AutoMapper;
using ExpressVoitures.Models.Entities;
using ExpressVoitures.Models.ViewModels;

namespace ExpressVoitures.Models.Profiles
{
    public class CarTrimProfile : Profile
    {
        public CarTrimProfile()
        {
            CreateMap<CarTrim, CarTrimViewModel>();
            CreateMap<CarTrimViewModel, CarTrim>();
        }
    }
}
