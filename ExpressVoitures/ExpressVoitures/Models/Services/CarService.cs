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
    public class CarService : GenericEntityService<Car, CarViewModel>, ICarService
    {
        private readonly IServiceScopeFactory _scopeFactory;

        public CarService(ICarRepository carRepository, IMapper mapper, IServiceScopeFactory scopeFactory) : base(carRepository, mapper)
        {
            _scopeFactory = scopeFactory;
        }

        public override void FillViewModel(CarViewModel viewModel)
        {
            base.FillViewModel(viewModel);

            if (viewModel == null)
                return;

            var trimService = _scopeFactory.CreateScope().ServiceProvider.GetService(typeof(ICarTrimService)) as ICarTrimService;
            if (trimService == null)
                return;

            var modelService = _scopeFactory.CreateScope().ServiceProvider.GetService(typeof(ICarModelService)) as ICarModelService;
            if (modelService == null)
                return;

            var makeService = _scopeFactory.CreateScope().ServiceProvider.GetService(typeof(ICarMakeService)) as ICarMakeService;
            if (makeService == null)
                return;

            viewModel.Trim = trimService.GetViewModel(viewModel.TrimId);
            if (viewModel.Trim != null)
            {
                viewModel.Trim.Model = modelService.GetViewModel(viewModel.Trim.ModelId);
                if (viewModel.Trim.Model != null)
                {
                    viewModel.Trim.Model.Make = makeService.GetViewModel(viewModel.Trim.Model.MakeId);
                }
            }
        }
        public override void FillCreateViewData(ViewDataDictionary viewData)
        {
            base.FillCreateViewData(viewData);

            viewData[ViewDataKeys.ExistingItems] = GetViewModels().OrderBy(m => m.Trim?.Model?.Make?.Name)
                .ThenBy(m => m.Trim?.Model?.Name)
                .ThenBy(m => m.Trim?.Name)
                .ToList();

            var makeService = _scopeFactory.CreateScope().ServiceProvider.GetService(typeof(ICarMakeService)) as ICarMakeService;
            if (makeService == null)
                return;
            var modelService = _scopeFactory.CreateScope().ServiceProvider.GetService(typeof(ICarModelService)) as ICarModelService;
            if (modelService == null)
                return;
            var trimService = _scopeFactory.CreateScope().ServiceProvider.GetService(typeof(ICarTrimService)) as ICarTrimService;
            if (trimService == null)
                return;

            var makes = makeService.GetViewModels().OrderBy(m => m.Name);
            viewData[ViewDataKeys.Makes] = new SelectList(makes, "Id", "Name");

            var models = modelService.GetViewModels().OrderBy(m => m.Name);
            viewData[ViewDataKeys.Models] = new SelectList(Enumerable.Empty<object>(), "Id", "Name");

            var trims = trimService.GetViewModels().OrderBy(m => m.Name);
            viewData[ViewDataKeys.Trims] = new SelectList(Enumerable.Empty<object>(), "Id", "Name");
        }
        public override void FillEditViewData(CarViewModel viewModel, ViewDataDictionary viewData)
        {
            base.FillEditViewData(viewModel, viewData);

            var makeService = _scopeFactory.CreateScope().ServiceProvider.GetService(typeof(ICarMakeService)) as ICarMakeService;
            if (makeService == null)
                return;
            var modelService = _scopeFactory.CreateScope().ServiceProvider.GetService(typeof(ICarModelService)) as ICarModelService;
            if (modelService == null)
                return;
            var trimService = _scopeFactory.CreateScope().ServiceProvider.GetService(typeof(ICarTrimService)) as ICarTrimService;
            if (trimService == null)
                return;
            var repairService = _scopeFactory.CreateScope().ServiceProvider.GetService(typeof(IRepairService)) as IRepairService;
            if (repairService == null)
                return;
            var saleService = _scopeFactory.CreateScope().ServiceProvider.GetService(typeof(ICarSaleService)) as ICarSaleService;
            if (saleService == null)
                return;

            var selectedMakeId = viewModel.Trim?.Model?.MakeId ?? 0;
            var makes = makeService.GetViewModels().OrderBy(m => m.Name).ToList();
            viewData[ViewDataKeys.Makes] = new SelectList(makes, "Id", "Name", selectedMakeId);

            var models = modelService.GetViewModels()
                .Where(m => m.MakeId == selectedMakeId)
                .OrderBy(m => m.Name)
                .ToList();
            viewData[ViewDataKeys.Models] = new SelectList(models, "Id", "Name", viewModel?.Trim?.ModelId);

            var selectedTrimId = viewModel?.Trim?.Model?.MakeId ?? 0;
            var trims = trimService.GetViewModels()
                .Where(m => m.ModelId == viewModel?.Trim?.ModelId)
                .OrderBy(m => m.Name)
                .ToList();
            viewData[ViewDataKeys.Trims] = new SelectList(trims, "Id", "Name", viewModel?.TrimId);

            var repairs = repairService.GetViewModels()
                .Where(r => r.CarId == viewModel?.Id)
                .OrderBy(r => r.Description)
                .ToList();
            viewData[ViewDataKeys.Repairs] = repairs;

            var sale = saleService.GetViewModels()
                .FirstOrDefault(r => r.CarId == viewModel?.Id);
            viewData[ViewDataKeys.Sales] = sale;
        }
    }
}
