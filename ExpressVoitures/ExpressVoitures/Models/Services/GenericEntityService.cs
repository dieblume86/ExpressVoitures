using AutoMapper;
using ExpressVoitures.Helpers;
using ExpressVoitures.Models.Repositories.Interfaces;
using ExpressVoitures.Models.Services.Interfaces;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.ComponentModel.DataAnnotations;

namespace ExpressVoitures.Models.Services
{
    public abstract class GenericEntityService<Entity, ViewModel> : IGenericEntityService<Entity, ViewModel>
        where Entity : class
        where ViewModel : class, new()
    {
        protected readonly IGenericRepository<Entity> _entityRepository;
        protected readonly IMapper _mapper;

        public GenericEntityService(IGenericRepository<Entity> repository, IMapper mapper)
        {
            _entityRepository = repository;
            _mapper = mapper;
        }


        public virtual Entity GetEntity(int id)
        {
            return _entityRepository.GetById(id);
        }
        public virtual List<Entity> GetAllEntities()
        {
            return _entityRepository.GetAll().ToList();
        }

        public virtual ViewModel GetViewModel(int id)
        {
            var viewModel = AutoMapToViewModel(GetEntity(id));

            if(viewModel == null)
                return new ViewModel();

            FillViewModel(viewModel);

            return viewModel;
        }
        public virtual List<ViewModel> GetViewModels()
        {
            List<ViewModel> viewModels = new();

            GetAllEntities().ForEach(entity =>
            {
                var viewModel = AutoMapToViewModel(entity);
                FillViewModel(viewModel);
                viewModels.Add(viewModel);
            });

            return viewModels;
        }


        public virtual int Add(ViewModel viewModel)
        {
            return _entityRepository.Add(AutoMapToEntity(viewModel));
        }

        public virtual void Update(ViewModel viewModel)
        {
            // Try to retrieve an Id from the ViewModel (convention "Id")
            var idProp = viewModel?.GetType().GetProperty("Id");
            if (idProp == null)
            {
                // No Id on the ViewModel: legacy behavior (map to a new entity)
                _entityRepository.Update(AutoMapToEntity(viewModel));
                return;
            }

            var idObj = idProp.GetValue(viewModel);
            int id = Convert.ToInt32(idObj ?? 0);

            if (id == 0)
            {
                // Id not provided: fallback to Add to avoid incorrect insertion
                _entityRepository.Add(AutoMapToEntity(viewModel));
                return;
            }

            // Retrieve the existing entity and map the VM values onto it
            var existing = _entityRepository.GetById(id);
            if (existing == null)
            {
                // If not found, create anyway (or throw an exception according to policy)
                _entityRepository.Add(AutoMapToEntity(viewModel));
                return;
            }

            // Map the properties from the ViewModel onto the existing entity — preserve the Id
            _mapper.Map(viewModel, existing);

            _entityRepository.Update(existing);
        }

        public virtual List<string> CheckModelErrors(ViewModel viewModel)
        {
            var modelErrors = new List<string>();

            CheckProductValidationResult(viewModel).ForEach(vr => modelErrors.Add(vr.ErrorMessage));

            return modelErrors;
        }
        public virtual List<ValidationResult> CheckProductValidationResult(ViewModel viewModel)
        {
            var context = new ValidationContext(viewModel);
            var results = new List<ValidationResult>();

            Validator.TryValidateObject(viewModel, context, results, true);

            return results;
        }

        public virtual void Delete(int id)
        {
            _entityRepository.Remove(id);
        }

        public Entity AutoMapToEntity(ViewModel viewModel)
        {
            return _mapper.Map<Entity>(viewModel);
        }
        public ViewModel AutoMapToViewModel(Entity entity)
        {
            return _mapper.Map<ViewModel>(entity);
        }

        public virtual void FillViewModel(ViewModel viewModel) { }
        public virtual void FillCreateViewData(ViewDataDictionary viewData) { }
        public virtual void FillEditViewData(ViewModel viewModel, ViewDataDictionary viewData) { }
    }
}
