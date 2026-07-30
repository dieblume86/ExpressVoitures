using ExpressVoitures.Data;
using ExpressVoitures.Models.Entities;
using ExpressVoitures.Models.Repositories.Interfaces;

namespace ExpressVoitures.Models.Repositories
{
    public class CarTrimRepository : GenericRepository<CarTrim>, ICarTrimRepository
    {
        public CarTrimRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
