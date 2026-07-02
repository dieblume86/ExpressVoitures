using ExpressVoitures.Controllers;
using ExpressVoitures.Data;
using ExpressVoitures.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace ExpressVoitures.Tests
{
    public class DbTest
    {
        [Fact]
        public void DbContext_AddCar()
        {
            var context = GetDBContext();

            var car = new Car
            {
                VinCode = "VIN123",
                Year = 2020,
                Make = "Toyota",
                Model = "Corolla",
                Trim = "LE"
            };


            Car? saved = null;

            try
            {
                context.Cars.Add(car);
                context.SaveChanges();

                Assert.Equal(1, context.Cars.Count());
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
        public async Task CarsController_AddCar()
        {
            // Arrange
            var context = GetDBContext();
            var controller = new CarsController(context);

            var car = new Car
            {
                VinCode = "VIN_CTRL",
                Year = 2021,
                Make = "Honda",
                Model = "Civic",
                Trim = "LX"
            };

            Car? saved = null;

            try
            {
                // Act
                var result = await controller.Create(car);

                // Assert - action should redirect on success
                Assert.IsType<RedirectToActionResult>(result);
                Assert.Equal(1, context.Cars.Count());
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
        public void DbContext_AddCarSale()
        {
            var context = GetDBContext();

            var carSale = new CarSale
            {
                CarId = 1,
                PurchaseDate = DateTime.Now,
                PurchasePrice = 15000.0f,
                AvailableForSaleDate = DateTime.Now.AddDays(30),
                SalePrice = 18000.0f,
                SaleDate = null
            };


            CarSale? saved = null;

            try
            {
                context.CarSales.Add(carSale);
                context.SaveChanges();

                Assert.Equal(1, context.CarSales.Count());
                saved = context.CarSales.First();
                Assert.Equal(1, saved.CarId);
            }
            finally
            {
                // Clean up the test data
                if (saved != null)
                {
                    context.CarSales.Remove(saved);
                    context.SaveChanges();
                }
            }
        }

        [Fact]
        public async Task CarSalesController_AddCarSale()
        {
            // Arrange
            var context = GetDBContext();
            var controller = new CarSalesController(context);

            var carSale = new CarSale
            {
                CarId = 1,
                PurchaseDate = DateTime.Now,
                PurchasePrice = 15000.0f,
                AvailableForSaleDate = DateTime.Now.AddDays(30),
                SalePrice = 18000.0f,
                SaleDate = null
            };


            CarSale? saved = null;

            try
            {
                // Act
                var result = await controller.Create(carSale);

                // Assert - action should redirect on success
                Assert.IsType<RedirectToActionResult>(result);
                Assert.Equal(1, context.CarSales.Count());
                saved = context.CarSales.First();
                Assert.Equal(1, saved.CarId);
            }
            finally
            {
                // Clean up the test data
                if (saved != null)
                {
                    context.CarSales.Remove(saved);
                    context.SaveChanges();
                }
            }
        }

        [Fact]
        public void DbContext_AddRepair()
        {
            var context = GetDBContext();

            var repair = new Repair
            {
                CarId = 1,
                Description = "Brake replacement",
                RepairCost = 300.0f
            };


            Repair? saved = null;

            try
            {
                context.Repairs.Add(repair);
                context.SaveChanges();

                Assert.Equal(1, context.Repairs.Count());
                saved = context.Repairs.First();
                Assert.Equal(1, saved.CarId);
            }
            finally
            {
                // Clean up the test data
                if (saved != null)
                {
                    context.Repairs.Remove(saved);
                    context.SaveChanges();
                }
            }
        }

        [Fact]
        public async Task RepairsController_AddRepair()
        {
            // Arrange
            var context = GetDBContext();
            var controller = new RepairsController(context);

            var repair = new Repair
            {
                CarId = 1,
                Description = "Brake replacement",
                RepairCost = 300.0f
            };


            Repair? saved = null;

            try
            {
                // Act
                var result = await controller.Create(repair);

                // Assert - action should redirect on success
                Assert.IsType<RedirectToActionResult>(result);
                Assert.Equal(1, context.Repairs.Count());
                saved = context.Repairs.First();
                Assert.Equal(1, saved.CarId);
            }
            finally
            {
                // Clean up the test data
                if (saved != null)
                {
                    context.Repairs.Remove(saved);
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
