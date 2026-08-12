using AutoMapper;
using ExpressVoitures.Models.Entities;
using ExpressVoitures.Models.Repositories.Interfaces;
using ExpressVoitures.Models.Services.Interfaces;
using Microsoft.Build.Tasks.Deployment.Bootstrapper;
using System.Collections.Generic;
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
            throw new NotImplementedException();
        }
        public virtual List<Entity> GetAllEntities()
        {
            List<Entity> entites = _entityRepository.GetAll().ToList();
            return entites;
        }

        public virtual ViewModel GetViewModel(int id)
        {
            throw new NotImplementedException();
        }
        public virtual List<ViewModel> GetViewModels()
        {
            List<ViewModel> viewModels = new();

            GetAllEntities().ForEach(entity => viewModels.Add(AutoMapToViewModel(entity)));

            return viewModels;
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
