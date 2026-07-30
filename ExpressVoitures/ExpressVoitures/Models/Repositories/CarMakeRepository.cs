using ExpressVoitures.Data;
using ExpressVoitures.Models.Entities;
using ExpressVoitures.Models.Repositories.Interfaces;

namespace ExpressVoitures.Models.Repositories
{
    public class CarMakeRepository : GenericRepository<CarMake>, ICarMakeRepository
    {
        public CarMakeRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
