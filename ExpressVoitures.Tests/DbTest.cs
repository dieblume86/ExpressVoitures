using ExpressVoitures.Data;
using ExpressVoitures.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace ExpressVoitures.Tests
{
    public class DbTest
    {
        [Fact]
        public void CreateCar_AddsCarToDbContext()
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
                if(saved != null)
                {
                    context.Cars.Remove(car);
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
