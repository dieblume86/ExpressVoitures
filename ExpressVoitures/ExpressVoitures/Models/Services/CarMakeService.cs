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
        public CarMakeService(ICarMakeRepository carMakeRepository, IMapper mapper) : base(carMakeRepository, mapper)
        {
        }

        public override List<string> CheckModelErrors(CarMakeViewModel viewModel)
        {
            var errors = base.CheckModelErrors(viewModel);

            if (AlreadyExists(viewModel.Name, out string error))
                errors.Add(error);

            return errors;
        }
        public override bool AlreadyExists<T>(T value, out string error)
        {
            string normalizedName = value.ToString().Trim();

            bool exists = _entityRepository.GetAll().Any(m =>
                m.Name.ToLower() == normalizedName.ToLower());

            error = exists ? $"Une marque de voiture avec le nom '{normalizedName}' existe déjà." : string.Empty;

            return exists;
        }

        public override void FillCreateViewData(ViewDataDictionary viewData)
        {
            base.FillCreateViewData(viewData);

            viewData[ViewDataKeys.ExistingItems] = GetViewModels().OrderBy(m => m.Name);
        }
    }
}
