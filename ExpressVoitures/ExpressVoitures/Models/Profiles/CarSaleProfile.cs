using AutoMapper;
using ExpressVoitures.Models.Entities;
using ExpressVoitures.Models.ViewModels;

namespace ExpressVoitures.Models.Profiles
{
    public class CarSaleProfile : Profile
    {
        public CarSaleProfile()
        {
            CreateMap<CarSale, CarSaleViewModel>();
            CreateMap<CarSaleViewModel, CarSale>();
        }
    }
}
