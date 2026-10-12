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
        public CarTrimService(ICarTrimRepository carTrimRepository, IMapper mapper, IServiceScopeFactory scopeFactory) : base(carTrimRepository, mapper, scopeFactory)
        {
        }

        public override void FillViewModel(CarTrimViewModel viewModel)
        {
            base.FillViewModel(viewModel);

            var makeService = _scopeFactory.CreateScope().ServiceProvider.GetService(typeof(ICarMakeService)) as ICarMakeService;
            if (makeService == null)
                return;
            var modelService = _scopeFactory.CreateScope().ServiceProvider.GetService(typeof(ICarModelService)) as ICarModelService;
            if (modelService == null)
                return;

            viewModel.Model = modelService.GetViewModel(viewModel.ModelId);
            viewModel.Model.Make = makeService.GetViewModel(viewModel.Model.MakeId);
        }
        public override void FillCreateViewData(ViewDataDictionary viewData)
        {
            base.FillCreateViewData(viewData);

            var makeService = _scopeFactory.CreateScope().ServiceProvider.GetService(typeof(ICarMakeService)) as ICarMakeService;
            if (makeService == null)
                return;
            var modelService = _scopeFactory.CreateScope().ServiceProvider.GetService(typeof(ICarModelService)) as ICarModelService;
            if (modelService == null)
                return;

            var makes = makeService.GetViewModels().OrderBy(m => m.Name);
            viewData[ViewDataKeys.Makes] = new SelectList(makes, "Id", "Name");

            var models = modelService.GetViewModels().OrderBy(m => m.Name);
            viewData[ViewDataKeys.Models] = new SelectList(Enumerable.Empty<object>(), "Id", "Name");

            viewData[ViewDataKeys.ExistingItems] = GetViewModels();
        }
        public override void FillEditViewData(CarTrimViewModel viewModel, ViewDataDictionary viewData)
        {
            base.FillEditViewData(viewModel, viewData);

            var makeService = _scopeFactory.CreateScope().ServiceProvider.GetService(typeof(ICarMakeService)) as ICarMakeService;
            if (makeService == null)
                return;
            var modelService = _scopeFactory.CreateScope().ServiceProvider.GetService(typeof(ICarModelService)) as ICarModelService;
            if (modelService == null)
                return;

            var modelVm = modelService.GetViewModel(viewModel.ModelId);
            var selectedMakeId = modelVm?.MakeId ?? 0;

            var makes = makeService.GetViewModels().OrderBy(m => m.Name).ToList();
            viewData[ViewDataKeys.Makes] = new SelectList(makes, "Id", "Name", selectedMakeId);

            var models = modelService.GetViewModels()
                .Where(m => m.MakeId == selectedMakeId)
                .OrderBy(m => m.Name)
                .ToList();
            viewData[ViewDataKeys.Models] = new SelectList(models, "Id", "Name", viewModel.ModelId);
        }
    }
}
