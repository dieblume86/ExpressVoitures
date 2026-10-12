using AutoMapper;
using ExpressVoitures.Helpers;
using ExpressVoitures.Models.Entities;
using ExpressVoitures.Models.Repositories.Interfaces;
using ExpressVoitures.Models.Services.Interfaces;
using ExpressVoitures.Models.ViewModels;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace ExpressVoitures.Models.Services
{
    public class CarMakeService : GenericEntityService<CarMake, CarMakeViewModel>, ICarMakeService
    {
        public CarMakeService(ICarMakeRepository carMakeRepository, IMapper mapper, IServiceScopeFactory scopeFactory) : base(carMakeRepository, mapper, scopeFactory)
        {
        }

        public override void FillCreateViewData(ViewDataDictionary viewData)
        {
            base.FillCreateViewData(viewData);

            viewData[ViewDataKeys.ExistingItems] = GetViewModels().OrderBy(m => m.Name);
        }
    }
}
