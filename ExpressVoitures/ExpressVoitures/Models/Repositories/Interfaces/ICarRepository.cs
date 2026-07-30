using ExpressVoitures.Models.Entities;

namespace ExpressVoitures.Models.Repositories.Interfaces
{
    public interface ICarRepository : IGenericRepository<Car>
    {
        IEnumerable<Car> GetPopularCars(int count);
    }
}
