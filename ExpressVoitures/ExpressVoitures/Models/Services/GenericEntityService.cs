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


        public virtual IEnumerable<Entity> GetAllEntities()
        {
            throw new NotImplementedException();
        }
        public virtual Entity GetEntity(int id)
        {
            throw new NotImplementedException();
        }
        public virtual ViewModel GetViewModel(int id)
        {
            throw new NotImplementedException();
        }
        public virtual IEnumerable<ViewModel> GetViewModels()
        {
            throw new NotImplementedException();
        }

        public virtual void Add(ViewModel viewModel)
        {
            var newEntity = AutoMapToEntity(viewModel);
            _entityRepository.Add(newEntity);
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
