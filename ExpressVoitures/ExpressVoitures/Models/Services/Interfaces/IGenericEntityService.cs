using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;

namespace ExpressVoitures.Models.Services.Interfaces
{
    public interface IGenericEntityService<Entity,ViewModel> 
        where Entity : class 
        where ViewModel : class
    {
        Entity GetEntity(int id);
        IEnumerable<Entity> GetAllEntities();
        ViewModel GetViewModel(int id);
        IEnumerable<ViewModel> GetViewModels();

        void Add(ViewModel viewModel);
        List<string> CheckModelErrors(ViewModel viewModel);
        List<ValidationResult> CheckProductValidationResult(ViewModel viewModel);

        void Delete(int id);
    }
}
