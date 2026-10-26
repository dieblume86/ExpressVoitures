using AspNetCoreGeneratedDocument;
using AutoMapper;
using ExpressVoitures.Helpers;
using ExpressVoitures.Models.Entities;
using ExpressVoitures.Models.Repositories.Interfaces;
using ExpressVoitures.Models.Services.Interfaces;
using ExpressVoitures.Models.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace ExpressVoitures.Models.Services
{
    public class CarModelService : GenericEntityService<CarModel, CarModelViewModel>, ICarModelService
    {
        private readonly ICarMakeService _carMakeService;

        public CarModelService(ICarModelRepository carModelRepository, IMapper mapper, ICarMakeService carMakeService) : base(carModelRepository, mapper)
        {
            _carMakeService = carMakeService;
        }

        public override bool AlreadyExists(CarModelViewModel viewModel, out string error)
        {
            var normalizedName = viewModel.Name.Trim();

            var exists = _entityRepository.GetAll().Where(m => m.MakeId == viewModel.MakeId).Any(m =>
                m.Name.ToLower() == normalizedName.ToLower());

            error = exists ? $"Un modèle de voiture avec le nom '{normalizedName}' existe déjà." : string.Empty;

            return exists;
        }

        public override void FillViewModel(CarModelViewModel viewModel)
        {
            base.FillViewModel(viewModel);

            viewModel.Make = _carMakeService.GetViewModel(viewModel.MakeId);
        }
        public override void FillCreateViewData(ViewDataDictionary viewData)
        {
            base.FillCreateViewData(viewData);

            var makes = _carMakeService.GetViewModels().OrderBy(m => m.Name);
            viewData[ViewDataKeys.Makes] = new SelectList(makes, "Id", "Name");

            viewData[ViewDataKeys.ExistingItems] = GetViewModels().OrderBy(m => m.Name);
        }

        public override void FillEditViewData(CarModelViewModel viewModel, ViewDataDictionary viewData)
        {
            base.FillEditViewData(viewModel, viewData);

            var makes = _carMakeService.GetViewModels().OrderBy(m => m.Name);
            viewData[ViewDataKeys.Makes] = new SelectList(makes, "Id", "Name");
        }
    }
}
