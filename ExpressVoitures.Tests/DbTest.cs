using AutoMapper;
using ExpressVoitures.Controllers;
using ExpressVoitures.Data;
using ExpressVoitures.Models.Entities;
using ExpressVoitures.Models.Profiles;
using ExpressVoitures.Models.Repositories;
using ExpressVoitures.Models.Services;
using ExpressVoitures.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Moq;
using System.Threading;

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
                // Clean up the test data
                if (saved != null)
                {
                    context.Cars.Remove(saved);
                    context.SaveChanges();
                }
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
            var context = GetDBContext();
            var repository = new CarMakeRepository(context);
            var loggerFactory = LoggerFactory.Create(builder => { });
            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<CarMakeProfile>();
            }, loggerFactory);

            var mapper = configuration.CreateMapper();

            var service = new CarMakeService(repository, mapper);
            var controller = new CarMakesController(service);

            var makeName = $"TestMake_{Guid.NewGuid():N}";

            var make = new CarMakeViewModel
            {
                Name = makeName
            };

            CarMake? saved = null;

            try
            {
                // Act
                controller.Create(make);

                // Assert - action should redirect on success
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
        public void CarMakesController_Delete_Make()
        {
            var context = GetDBContext();
            var repository = new CarMakeRepository(context);
            var loggerFactory = LoggerFactory.Create(builder => { });

            var mapperMock = new Mock<IMapper>();

            var service = new CarMakeService(repository, mapperMock.Object);
            var controller = new CarMakesController(service);

            var makeName = $"TestMake_{Guid.NewGuid():N}";

            var make = new CarMake
            {
                Name = makeName
            };

            CarMake? saved = null;

            try
            {
                // Act
                context.CarMakes.Add(make);
                context.SaveChanges();

                saved = context.CarMakes.FirstOrDefault(x => x.Name == makeName);

                controller.Delete(saved.Id);
                saved = context.CarMakes.FirstOrDefault(x => x.Name == makeName);

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
        //[Fact]
        //public async Task CarModelsController_AddModel()
        //{
        //    var context = GetDBContext();
        //    var controller = new CarModelsController(context);
        //    var modelName = "Test Model";

        //    var model = new CarModel
        //    {
        //        Name = modelName
        //    };

        //    CarModel? saved = null;

        //    try
        //    {
        //        // Act
        //        var result = await controller.Create(model);

        //        // Assert - action should redirect on success
        //        Assert.IsType<RedirectToActionResult>(result);
        //        Assert.NotEqual(0, context.CarModels.Count());
        //        saved = context.CarModels.FirstOrDefault(x => x.Name == modelName);
        //        Assert.Equal(modelName, saved.Name);
        //    }
        //    finally
        //    {
        //        // Clean up the test data
        //        if (saved != null)
        //        {
        //            context.CarModels.Remove(saved);
        //            context.SaveChanges();
        //        }
        //    }
        //}
        [Fact]
        public async Task CarTrimsController_Add_Trim()
        {
            var context = GetDBContext();
            var controller = new CarTrimsController(context);
            var trimName = "Test Trim";

            var trim = new CarTrim
            {
                Name = trimName
            };

            CarTrim? saved = null;

            try
            {
                // Act
                var result = await controller.Create(trim);

                // Assert - action should redirect on success
                Assert.IsType<RedirectToActionResult>(result);
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
        public async Task CarsController_Add_Car()
        {
            // Arrange
            var context = GetDBContext();
            var controller = new CarsController(context);

            var car = new Car
            {
                VinCode = "VIN_CTRL",
                Year = 2021
            };

            Car? saved = null;

            try
            {
                // Act
                var result = await controller.Create(car);

                // Assert - action should redirect on success
                Assert.IsType<RedirectToActionResult>(result);
                Assert.NotEqual(0, context.Cars.Count());
                saved = context.Cars.First();
                Assert.Equal("VIN_CTRL", saved.VinCode);
            }
            finally
            {
                // Clean up the test data
                if (saved != null)
                {
                    context.Cars.Remove(saved);
                    context.SaveChanges();
                }
            }
        }
        [Fact]
        public async Task RepairsController_Add_Repair()
        {
            // Arrange
            var context = GetDBContext();
            var controller = new RepairsController(context);


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

                // Act
                var result = await controller.Create(repair);

                // Assert - action should redirect on success
                Assert.IsType<RedirectToActionResult>(result);
                Assert.NotEqual(0, context.Repairs.Count());
                saved = context.Repairs.First();
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
        public async Task CarSalesController_Add_CarSale()
        {
            // Arrange
            var context = GetDBContext();
            var controller = new CarSalesController(context);
            var purachaseDate = DateTime.Now;

            var car = new Car
            {
                VinCode = "VIN123",
                Year = 2020
            };


            Car? carSaved = null;
            CarSale? saved = null;

            try
            {
                // Act
                context.Cars.Add(car);
                context.SaveChanges();

                Assert.NotEqual(0, context.Cars.Count());
                carSaved = context.Cars.FirstOrDefault(x => x.VinCode == car.VinCode);
                Assert.Equal("VIN123", carSaved.VinCode);

                var carSale = new CarSale
                {
                    CarId = carSaved.Id,
                    PurchaseDate = purachaseDate,
                    PurchasePrice = 15000.0f,
                    AvailableForSaleDate = DateTime.Now.AddDays(30),
                    SalePrice = 18000.0f,
                    SaleDate = null
                };

                var result = await controller.Create(carSale);

                // Assert - action should redirect on success
                Assert.IsType<RedirectToActionResult>(result);
                Assert.NotEqual(0, context.CarSales.Count());
                saved = context.CarSales.First();
                Assert.Equal(purachaseDate, saved.PurchaseDate);
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
    }
}
