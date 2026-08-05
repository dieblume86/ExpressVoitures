using AutoMapper;
using ExpressVoitures.Models.Entities;
using ExpressVoitures.Models.ViewModels;

namespace ExpressVoitures.Models.Profiles
{
    public class RepairProfile : Profile
    {
        public RepairProfile()
        {
            CreateMap<Repair, RepairViewModel>();
            CreateMap<RepairViewModel, Repair>();
        }
    }
}
