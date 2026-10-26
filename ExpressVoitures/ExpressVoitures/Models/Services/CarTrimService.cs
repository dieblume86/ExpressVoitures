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
    public class CarTrimService : GenericEntityService<CarTrim, CarTrimViewModel>, ICarTrimService
    {
        private readonly ICarMakeService _carMakeService;
        private readonly ICarModelService _carModelService;

        public CarTrimService(ICarTrimRepository carTrimRepository, IMapper mapper, ICarMakeService carMakeService, ICarModelService carModelService) : base(carTrimRepository, mapper)
        {
            _carMakeService = carMakeService;
            _carModelService = carModelService;
        }

        public override bool AlreadyExists(CarTrimViewModel viewModel, out string error)
        {
            var normalizedName = viewModel.Name.Trim();

            var exists = _entityRepository.GetAll().Where(m => m.ModelId == viewModel.ModelId).Any(m =>
                m.Name.ToLower() == normalizedName.ToLower());

            error = exists ? $"Une finition de voiture avec le nom '{normalizedName}' existe déjà." : string.Empty;

            return exists;
        }

        public override void FillViewModel(CarTrimViewModel viewModel)
        {
            base.FillViewModel(viewModel);

            viewModel.Model = _carModelService.GetViewModel(viewModel.ModelId);
            viewModel.Model.Make = _carMakeService.GetViewModel(viewModel.Model.MakeId);
        }
        public override void FillCreateViewData(ViewDataDictionary viewData)
        {
            base.FillCreateViewData(viewData);

            var makes = _carMakeService.GetViewModels().OrderBy(m => m.Name);
            viewData[ViewDataKeys.Makes] = new SelectList(makes, "Id", "Name");

            var models = _carModelService.GetViewModels().OrderBy(m => m.Name);
            viewData[ViewDataKeys.Models] = new SelectList(Enumerable.Empty<object>(), "Id", "Name");

            viewData[ViewDataKeys.ExistingItems] = GetViewModels().OrderBy(m => m.Name);
        }
        public override void FillEditViewData(CarTrimViewModel viewModel, ViewDataDictionary viewData)
        {
            base.FillEditViewData(viewModel, viewData);

            var modelVm = _carModelService.GetViewModel(viewModel.ModelId);
            var selectedMakeId = modelVm?.MakeId ?? 0;

            var makes = _carMakeService.GetViewModels().OrderBy(m => m.Name).ToList();
            viewData[ViewDataKeys.Makes] = new SelectList(makes, "Id", "Name", selectedMakeId);

            var models = _carModelService.GetViewModels()
                .Where(m => m.MakeId == selectedMakeId)
                .OrderBy(m => m.Name)
                .ToList();
            viewData[ViewDataKeys.Models] = new SelectList(models, "Id", "Name", viewModel.ModelId);
        }
    }
}
