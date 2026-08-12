using AutoMapper;
using ExpressVoitures.Controllers;
using ExpressVoitures.Data;
using ExpressVoitures.Models.Entities;
using ExpressVoitures.Models.Profiles;
using ExpressVoitures.Models.Repositories;
using ExpressVoitures.Models.Services;
using ExpressVoitures.Models.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;

namespace ExpressVoitures.Tests
{
    public class DbTest
    {
        [Fact]
        public void DbContext_AddMake()
        {
            var context = GetDBContext();
            var makeName = "Test Make";

            var make = new CarMake
            {
                Name = makeName
            };

            CarMake? saved = null;

            try
            {
                context.CarMakes.Add(make);
                context.SaveChanges();
                Assert.NotEqual(0, context.CarMakes.Count());
                saved = context.CarMakes.FirstOrDefault(x => x.Name == makeName);
                Assert.Equal(makeName, saved.Name);
            }
            finally
            {
                // Clean up the test data
                if (saved != null)
                {
                    context.CarMakes.Remove(saved);
                    context.SaveChanges();
                }
            }
        }
        [Fact]
        public void DbContext_AddModel()
        {
            var context = GetDBContext();
            var modelName = "Test Model";
            var model = new CarModel
            {
                Name = modelName
            };
            CarModel? saved = null;
            try
            {
                context.CarModels.Add(model);
                context.SaveChanges();
                Assert.NotEqual(0, context.CarModels.Count());
                saved = context.CarModels.FirstOrDefault(x => x.Name == modelName);
                Assert.Equal(modelName, saved.Name);
            }
            finally
            {
                // Clean up the test data
                if (saved != null)
                {
                    context.CarModels.Remove(saved);
                    context.SaveChanges();
                }
            }
        }
        [Fact]
        public void DbContext_AddTrim()
        {
            var context = GetDBContext();
            var trimName = "Test Trim";
            var trim = new CarTrim
            {
                Name = trimName
            };
            CarTrim? saved = null;
            try
            {
                context.CarTrims.Add(trim);
                context.SaveChanges();
                Assert.NotEqual(0, context.CarTrims.Count());
                saved = context.CarTrims.FirstOrDefault(x => x.Name == trimName);
                Assert.Equal(trimName, saved.Name);
            }
            finally
            {
                // Clean up the test data
                if (saved != null)
                {
                    context.CarTrims.Remove(saved);
                    context.SaveChanges();
                }
            }
        }
        [Fact]
        public void DbContext_AddCar()
        {
            var context = GetDBContext();

            var car = new Car
            {
                VinCode = "VIN123",
                Year = 2020
            };


            Car? saved = null;

            try
            {
                context.Cars.Add(car);
                context.SaveChanges();

                Assert.NotEqual(0, context.Cars.Count());
                saved = context.Cars.First();
                Assert.Equal("VIN123", saved.VinCode);
            }
            finally
            {
                //// Clean up the test data
                //if (saved != null)
                //{
                //    context.Cars.Remove(saved);
                //    context.SaveChanges();
                //}
            }
        }
        [Fact]
        public void DbContext_AddRepair()
        {
            var context = GetDBContext();

            var car = new Car
            {
                VinCode = "VIN123",
                Year = 2020
            };


            Car? carSaved = null;
            Repair? saved = null;

            try
            {
                context.Cars.Add(car);
                context.SaveChanges();

                Assert.NotEqual(0, context.Cars.Count());
                carSaved = context.Cars.FirstOrDefault(x => x.VinCode == car.VinCode);
                Assert.Equal("VIN123", carSaved.VinCode);


                var repair = new Repair
                {
                    CarId = carSaved.Id,
                    Description = "Brake replacement",
                    RepairCost = 300.0f
                };


                context.Repairs.Add(repair);
                context.SaveChanges();

                Assert.NotEqual(0, context.Repairs.Count());
                saved = context.Repairs.FirstOrDefault(x => x.Description == repair.Description);
                Assert.Equal(carSaved.Id, saved.CarId);
            }
            finally
            {
                // Clean up the test data
                if (carSaved != null)
                {
                    context.Cars.Remove(carSaved);
                    context.SaveChanges();
                }
            }
        }
        [Fact]
        public void DbContext_AddCarSale()
        {
            var context = GetDBContext();
            var purchaseDate = DateTime.Now;

            var car = new Car
            {
                VinCode = "VIN123",
                Year = 2020
            };



            Car? carSaved = null;
            CarSale? carSaleSaved = null;

            try
            {
                context.Cars.Add(car);
                context.SaveChanges();

                Assert.NotEqual(0, context.Cars.Count());
                carSaved = context.Cars.FirstOrDefault(x => x.VinCode == car.VinCode);
                Assert.Equal("VIN123", carSaved.VinCode);

                var carSale = new CarSale
                {
                    CarId = carSaved.Id,
                    PurchaseDate = purchaseDate,
                    PurchasePrice = 15000.0f,
                    AvailableForSaleDate = DateTime.Now.AddDays(30),
                    SalePrice = 18000.0f,
                    SaleDate = null
                };

                context.CarSales.Add(carSale);
                context.SaveChanges();

                Assert.NotEqual(0, context.CarSales.Count());
                carSaleSaved = context.CarSales.First();
                Assert.Equal(purchaseDate, carSaleSaved.PurchaseDate);
            }
            finally
            {
                // Clean up the test data
                if (carSaved != null)
                {
                    context.Cars.Remove(carSaved);
                    context.SaveChanges();
                }
            }
        }


        [Fact]
        public void CarMakesController_Add_Make()
        {
            // Arrange
            var context = GetDBContext();
            var repository = new CarMakeRepository(context);
            var mapper = GetAutoMapper<CarMakeProfile>();

            var service = new CarMakeService(repository, mapper);
            var controller = new CarMakesController(service);

            var target = $"Test_{Guid.NewGuid():N}";

            var viewModel = new CarMakeViewModel
            {
                Name = target
            };

            CarMake? saved = null;

            try
            {
                // Act
                controller.Create(viewModel);

                // Assert
                saved = context.CarMakes.FirstOrDefault(x => x.Name == target);
                Assert.Equal(saved.Name, target);
            }
            finally
            {
                // Clean up the test data
                if (saved != null)
                {
                    context.CarMakes.Remove(saved);
                    context.SaveChanges();
                }
            }
        }
        [Fact]
        public void CarModelsController_Add_Model()
        {
            // Arrange
            var context = GetDBContext();
            var repository = new CarModelRepository(context);
            var mapper = GetAutoMapper<CarModelProfile>();

            var service = new CarModelService(repository, mapper);
            var controller = new CarModelsController(service);

            var target = $"Test_{Guid.NewGuid():N}";

            var entity = new CarMake
            {
                Name = target
            };

            CarMake? entitySaved = null;
            CarModel? saved = null;

            try
            {
                // Arrange
                context.CarMakes.Add(entity);
                context.SaveChanges();

                entitySaved = context.CarMakes.FirstOrDefault(x => x.Name == target);

                var viewModel = new CarModelViewModel
                {
                    Name = target,
                    MakeId = entitySaved.Id
                };

                // Act
                controller.Create(viewModel);

                // Assert
                saved = context.CarModels.FirstOrDefault(x => x.MakeId == entitySaved.Id);
                Assert.Equal(saved.MakeId, entitySaved.Id);
            }
            finally
            {
                // Clean up the test data
                if (entitySaved != null)
                {
                    context.CarMakes.Remove(entitySaved);
                    context.SaveChanges();
                }

                saved = context.CarModels.FirstOrDefault(x => x.MakeId == entitySaved.Id);

                if (saved != null)
                {
                    context.CarModels.Remove(saved);
                    context.SaveChanges();
                }
            }
        }
        [Fact]
        public void CarSalesController_Add_Sale()
        {
            // Arrange
            var context = GetDBContext();
            var repository = new CarSaleRepository(context);
            var mapper = GetAutoMapper<CarSaleProfile>();

            var service = new CarSaleService(repository, mapper);
            var controller = new CarSalesController(service);

            var target = $"Test_{Guid.NewGuid():N}";

            Car? carSaved = null;
            CarSale? saved = null;

            try
            {
                //Arrange
                var car = new Car
                {
                    VinCode = target
                };
                context.Cars.Add(car);
                context.SaveChanges();

                carSaved = context.Cars.FirstOrDefault(x => x.VinCode == target);

                var viewModel = new CarSaleViewModel
                {
                    PurchaseDate = DateTimeOffset.Now,
                    PurchasePrice = 15000.0f,
                    SalePrice = 20000.0f,
                    CarId = carSaved.Id
                };

                // Act
                controller.Create(viewModel);

                // Assert
                saved = context.CarSales.FirstOrDefault(x => x.CarId == carSaved.Id);
                Assert.Equal(saved.CarId, carSaved.Id);
            }
            finally
            {
                // Clean up the test data
                if (carSaved != null)
                {
                    context.Cars.Remove(carSaved);
                    context.SaveChanges();
                }

                saved = context.CarSales.FirstOrDefault(x => x.CarId == carSaved.Id);

                if (saved != null)
                {
                    context.CarSales.Remove(saved);
                    context.SaveChanges();
                }
            }
        }



        [Fact]
        public void CarMakesController_Delete_Make()
        {
            var context = GetDBContext();
            var repository = new CarMakeRepository(context);
            var loggerFactory = LoggerFactory.Create(builder => { });

            var mapperMock = new Mock<IMapper>();

            var service = new CarMakeService(repository, mapperMock.Object);
            var controller = new CarMakesController(service);

            var target = $"Test_{Guid.NewGuid():N}";

            var entity = new CarMake
            {
                Name = target
            };

            CarMake? saved = null;

            try
            {
                // Act
                context.CarMakes.Add(entity);
                context.SaveChanges();

                saved = context.CarMakes.FirstOrDefault(x => x.Name == target);

                controller.Delete(saved.Id);
                saved = context.CarMakes.FirstOrDefault(x => x.Name == target);

                // Assert - action should redirect on success
                Assert.Null(saved);
            }
            finally
            {
                // Clean up the test data
                if (saved != null)
                {
                    context.CarMakes.Remove(saved);
                    context.SaveChanges();
                }
            }
        }


        private ApplicationDbContext GetDBContext()
        {
            var appSettingsPath = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json");

            var configBuilder = new ConfigurationBuilder();
            configBuilder.AddJsonFile(appSettingsPath);

            // fallback to environment variables
            var configuration = configBuilder.Build();

            var connectionString = configuration.GetConnectionString("DefaultConnection");

            Assert.False(string.IsNullOrWhiteSpace(connectionString), "Connection string 'DefaultConnection' not found.");

            // Configure DbContextOptions to point to the real database
            var options = new DbContextOptionsBuilder<ApplicationDbContext>().UseSqlServer(connectionString).Options;

            // Create context (constructor requires IConfiguration)
            var context = new ApplicationDbContext(options);

            return context;
        }
        private IMapper GetAutoMapper<TProfile>() where TProfile : Profile, new()
        {
            var loggerFactory = LoggerFactory.Create(builder => { });
            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<TProfile>();
            }, loggerFactory);

            var mapper = configuration.CreateMapper();
            return mapper;
        }
    }
}
