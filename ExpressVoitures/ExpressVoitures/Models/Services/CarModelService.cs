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
        public CarModelService(ICarModelRepository carModelRepository, IMapper mapper, IServiceScopeFactory scopeFactory) : base(carModelRepository, mapper, scopeFactory)
        {
        }


        public override void FillViewModel(CarModelViewModel viewModel)
        {
            base.FillViewModel(viewModel);

            var makeService = _scopeFactory.CreateScope().ServiceProvider.GetService(typeof(ICarMakeService)) as ICarMakeService;
            if (makeService == null)
                return;

            viewModel.Make = makeService.GetViewModel(viewModel.MakeId);
        }
        public override void FillCreateViewData(ViewDataDictionary viewData)
        {
            base.FillCreateViewData(viewData);

            var makeService = _scopeFactory.CreateScope().ServiceProvider.GetService(typeof(ICarMakeService)) as ICarMakeService;
            if (makeService == null)
                return;

            var makes = makeService.GetViewModels().OrderBy(m => m.Name);
            viewData[ViewDataKeys.Makes] = new SelectList(makes, "Id", "Name");

            viewData[ViewDataKeys.ExistingItems] = GetViewModels();
        }

        public override void FillEditViewData(CarModelViewModel viewModel, ViewDataDictionary viewData)
        {
            base.FillEditViewData(viewModel, viewData);

            var makeService = _scopeFactory.CreateScope().ServiceProvider.GetService(typeof(ICarMakeService)) as ICarMakeService;
            if (makeService == null)
                return;

            var makes = makeService.GetViewModels().OrderBy(m => m.Name);
            viewData[ViewDataKeys.Makes] = new SelectList(makes, "Id", "Name");
        }
    }
}
