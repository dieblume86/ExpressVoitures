using AutoMapper;
using ExpressVoitures.Models.Entities;
using ExpressVoitures.Models.Repositories.Interfaces;
using ExpressVoitures.Models.Services.Interfaces;
using ExpressVoitures.Models.ViewModels;
namespace ExpressVoitures.Models.Services
{
    public class CarSaleService : GenericEntityService<CarSale, CarSaleViewModel>, ICarSaleService
    {
        // Avoid circular dependency by using IServiceScopeFactory to resolve IRepairService when needed
        private readonly IServiceScopeFactory _scopeFactory;

        public CarSaleService(ICarSaleRepository carSaleRepository, IMapper mapper, IServiceScopeFactory scopeFactory) : base(carSaleRepository, mapper)
        {
            _scopeFactory = scopeFactory;
        }

        public override void FillViewModel(CarSaleViewModel viewModel)
        {
            base.FillViewModel(viewModel);

            var repairService = _scopeFactory.CreateScope().ServiceProvider.GetService(typeof(IRepairService)) as IRepairService;
            if (repairService == null)
                return;
            
            var salePrice = 0f;

            var repairs = repairService.GetViewModels().Where(r => r.CarId == viewModel.CarId).ToList();
            if (repairs.Count > 0)
            {
                foreach (var repair in repairs)
                {
                    salePrice += repair.RepairCost;
                }
            }
            salePrice += viewModel.PurchasePrice + 500f;

            viewModel.SalePrice = salePrice;
        }
    }
}
