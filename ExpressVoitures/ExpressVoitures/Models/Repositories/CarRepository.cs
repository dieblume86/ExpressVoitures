using ExpressVoitures.Data;
using ExpressVoitures.Models.Entities;
using ExpressVoitures.Models.Repositories.Interfaces;

namespace ExpressVoitures.Models.Repositories
{
    public class CarRepository : GenericRepository<Car>, ICarRepository
    {
        public CarRepository(ApplicationDbContext context) : base(context)
        {
        }
        public IEnumerable<Car> GetPopularCars(int count)
        {
            throw new NotImplementedException();
        }
    }
}
