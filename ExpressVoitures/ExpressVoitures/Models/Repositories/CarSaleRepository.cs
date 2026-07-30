using ExpressVoitures.Data;
using ExpressVoitures.Models.Entities;
using ExpressVoitures.Models.Repositories.Interfaces;

namespace ExpressVoitures.Models.Repositories
{
    public class CarSaleRepository : GenericRepository<CarSale>, ICarSaleRepository
    {
        public CarSaleRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
