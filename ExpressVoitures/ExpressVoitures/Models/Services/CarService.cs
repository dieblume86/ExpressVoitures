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
        private readonly ICarTrimService _carTrimService;
        private readonly ICarMakeService _carMakeService;
        private readonly ICarModelService _carModelService;
        private readonly IRepairService _repairService;
        private readonly ICarSaleService _carSaleService;

        public CarService(ICarRepository carRepository, IMapper mapper, ICarMakeService carMakeService, ICarModelService carModelService, ICarTrimService carTrimService, IRepairService repairService, ICarSaleService carSaleService) : base(carRepository, mapper)
        {  
            this._carMakeService = carMakeService;
            this._carModelService = carModelService;
            this._carTrimService = carTrimService;
            this._repairService = repairService;
            this._carSaleService = carSaleService;
        }

        public override bool AlreadyExists(CarViewModel viewModel, out string error)
        {
            var normalizedName = viewModel.VinCode.Trim();
            
            var exists = _entityRepository.GetAll().Any(m =>
                m.VinCode.ToLower() == normalizedName.ToLower() && m.Id != viewModel.Id);
           
            error = exists ? $"Une voiture avec le code VIN '{normalizedName}' existe déjà." : string.Empty;
            
            return exists;
        }

        public override void FillViewModel(CarViewModel viewModel)
        {
            base.FillViewModel(viewModel);

            if (viewModel == null)
                return;

            viewModel.Trim = _carTrimService.GetViewModel(viewModel.TrimId);
            if (viewModel.Trim != null)
            {
                viewModel.Trim.Model = _carModelService.GetViewModel(viewModel.Trim.ModelId);
                if (viewModel.Trim.Model != null)
                {
                    viewModel.Trim.Model.Make = _carMakeService.GetViewModel(viewModel.Trim.Model.MakeId);
                }
            }

            viewModel.Sale = _carSaleService.GetViewModels().FirstOrDefault(s => s.CarId == viewModel.Id);
        }
        public override void FillCreateViewData(ViewDataDictionary viewData)
        {
            base.FillCreateViewData(viewData);

            viewData[ViewDataKeys.ExistingItems] = GetViewModels().OrderBy(m => m.Trim?.Model?.Make?.Name)
                .ThenBy(m => m.Trim?.Model?.Name)
                .ThenBy(m => m.Trim?.Name)
                .ToList();

            var makes = _carMakeService.GetViewModels().OrderBy(m => m.Name);
            viewData[ViewDataKeys.Makes] = new SelectList(makes, "Id", "Name");

            var models = _carModelService.GetViewModels().OrderBy(m => m.Name);
            viewData[ViewDataKeys.Models] = new SelectList(Enumerable.Empty<object>(), "Id", "Name");

            var trims = _carTrimService.GetViewModels().OrderBy(m => m.Name);
            viewData[ViewDataKeys.Trims] = new SelectList(Enumerable.Empty<object>(), "Id", "Name");
        }
        public override void FillEditViewData(CarViewModel viewModel, ViewDataDictionary viewData)
        {
            base.FillEditViewData(viewModel, viewData);

            var selectedMakeId = viewModel.Trim?.Model?.MakeId ?? 0;
            var makes = _carMakeService.GetViewModels().OrderBy(m => m.Name).ToList();
            viewData[ViewDataKeys.Makes] = new SelectList(makes, "Id", "Name", selectedMakeId);

            var models = _carModelService.GetViewModels()
                .Where(m => m.MakeId == selectedMakeId)
                .OrderBy(m => m.Name)
                .ToList();
            viewData[ViewDataKeys.Models] = new SelectList(models, "Id", "Name", viewModel?.Trim?.ModelId);

            var selectedTrimId = viewModel?.Trim?.Model?.MakeId ?? 0;
            var trims = _carTrimService.GetViewModels()
                .Where(m => m.ModelId == viewModel?.Trim?.ModelId)
                .OrderBy(m => m.Name)
                .ToList();
            viewData[ViewDataKeys.Trims] = new SelectList(trims, "Id", "Name", viewModel?.TrimId);

            var repairs = _repairService.GetViewModels()
                .Where(r => r.CarId == viewModel?.Id)
                .OrderBy(r => r.Description)
                .ToList();
            viewData[ViewDataKeys.Repairs] = repairs;

            var sale = _carSaleService.GetViewModels()
                .FirstOrDefault(r => r.CarId == viewModel?.Id);
            viewData[ViewDataKeys.Sales] = sale;
        }
    }
}
