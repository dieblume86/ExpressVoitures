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

        //public override List<CarModelViewModel> GetViewModels()
        //{
        //    var viewModels = base.GetViewModels();

        //    foreach (var viewModel in viewModels)
        //    {
        //        viewModel.LoadCarMake(id => _mapper.Map<CarMakeViewModel>(_repository.GetById(id).CarMake));
        //    }

        //    return viewModels;
        //}
    }
}
