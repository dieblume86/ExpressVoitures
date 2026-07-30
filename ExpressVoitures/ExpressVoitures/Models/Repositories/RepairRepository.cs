using ExpressVoitures.Data;
using ExpressVoitures.Models.Entities;
using ExpressVoitures.Models.Repositories.Interfaces;

namespace ExpressVoitures.Models.Repositories
{
    public class RepairRepository : GenericRepository<Repair>, IRepairRepository
    {
        public RepairRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
