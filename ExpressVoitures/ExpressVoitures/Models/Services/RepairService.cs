using AutoMapper;
using ExpressVoitures.Models.Entities;
using ExpressVoitures.Models.Repositories.Interfaces;
using ExpressVoitures.Models.Services.Interfaces;
using ExpressVoitures.Models.ViewModels;
namespace ExpressVoitures.Models.Services
{
    public class RepairService : GenericEntityService<Repair, RepairViewModel>, IRepairService
    {
        public RepairService(IRepairRepository repairRepository, IMapper mapper) : base(repairRepository, mapper)
        {
        }
    }
}
