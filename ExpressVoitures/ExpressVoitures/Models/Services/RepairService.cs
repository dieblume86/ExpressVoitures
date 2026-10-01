using AutoMapper;
using ExpressVoitures.Models.Entities;
using ExpressVoitures.Models.Repositories.Interfaces;
using ExpressVoitures.Models.Services.Interfaces;
using ExpressVoitures.Models.ViewModels;

namespace ExpressVoitures.Models.Services
{
    public class RepairService : GenericEntityService<Repair, RepairViewModel>, IRepairService
    {
        // Avoid circular dependency by using IServiceScopeFactory to resolve ICarSaleService when needed
        private readonly IServiceScopeFactory _scopeFactory;

        public RepairService(IRepairRepository repairRepository, IMapper mapper, IServiceScopeFactory scopeFactory)
            : base(repairRepository, mapper)
        {
            _scopeFactory = scopeFactory;
        }

        public override void Add(RepairViewModel viewModel)
        {
            base.Add(viewModel);
            // After adding, recalculate the related car sale if it exists
            try
            {
                UpdateRelatedCarSale(viewModel.CarId);
            }
            catch
            {
                // do not fail the repair creation if updating the car sale fails
            }
        }

        public override void Update(RepairViewModel viewModel)
        {
            base.Update(viewModel);
            try
            {
                UpdateRelatedCarSale(viewModel.CarId);
            }
            catch
            {
            }
        }

        public override void Delete(int id)
        {
            // retrieve carId before deletion
            int carId = 0;
            try
            {
                var vm = GetViewModel(id);
                if (vm != null)
                {
                    carId = vm.CarId;
                }
            }
            catch
            {
                // ignore
            }

            base.Delete(id);

            if (carId > 0)
            {
                try
                {
                    UpdateRelatedCarSale(carId);
                }
                catch
                {
                }
            }
        }

        private void UpdateRelatedCarSale(int carId)
        {
            // Check if the car has a sale record and update it accordingly
            // Temporarily resolve ICarSaleService within a scope to avoid circular dependency
            var carSaleService = _scopeFactory.CreateScope().ServiceProvider.GetService(typeof(ICarSaleService)) as ICarSaleService;
            if (carSaleService == null)
                return;

            var saleVm = carSaleService.GetViewModels().FirstOrDefault(s => s.CarId == carId);
            if (saleVm == null)
                return;

            // Recalculate and save
            carSaleService.FillViewModel(saleVm);
            carSaleService.Update(saleVm);
        }
    }
}