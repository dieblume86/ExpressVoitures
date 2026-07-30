using ExpressVoitures.Data;
using ExpressVoitures.Models.Entities;
using ExpressVoitures.Models.Repositories.Interfaces;

namespace ExpressVoitures.Models.Repositories
{
    public class CarModelRepository : GenericRepository<CarModel>, ICarModelRepository
    {
        public CarModelRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
