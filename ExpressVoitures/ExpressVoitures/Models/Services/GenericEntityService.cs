using AutoMapper;
using ExpressVoitures.Models.Repositories.Interfaces;
using ExpressVoitures.Models.Services.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace ExpressVoitures.Models.Services
{
    public abstract class GenericEntityService<Entity, ViewModel> : IGenericEntityService<Entity, ViewModel>
        where Entity : class
        where ViewModel : class
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
           return AutoMapToViewModel(GetEntity(id));
        }
        public virtual List<ViewModel> GetViewModels()
        {
            List<ViewModel> viewModels = new();

            GetAllEntities().ForEach(entity => viewModels.Add(AutoMapToViewModel(entity)));

            return viewModels;
        }


        public virtual void Add(ViewModel viewModel)
        {
            _entityRepository.Add(AutoMapToEntity(viewModel));
        }

        public virtual void Update(ViewModel viewModel)
        {
            _entityRepository.Update(AutoMapToEntity(viewModel));
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
    }
}
