using ExpressVoitures.Models.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ExpressVoitures.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }


        public DbSet<CarMake> CarMakes { get; set; }
        public DbSet<CarModel> CarModels { get; set; }
        public DbSet<CarTrim> CarTrims { get; set; }
        public DbSet<Car> Cars { get; set; }
        public DbSet<Repair> Repairs { get; set; }
        public DbSet<CarSale> CarSales { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=SchoolDb;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<CarMake>()
                    .HasMany(make => make.Models)
                    .WithMany(model => model.Makes);

            modelBuilder.Entity<CarModel>()
                    .HasMany(model => model.Makes)
                    .WithMany(make => make.Models);

            modelBuilder.Entity<CarTrim>()
               .HasMany(t => t.Makes)
               .WithMany(m => m.Trims);

            modelBuilder.Entity<Car>(entity =>
            {
                entity.HasOne(c => c.Make)
                    .WithMany(m => m.Cars)
                    .HasForeignKey(c => c.MakeId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(c => c.Model)
                    .WithMany(m => m.Cars)
                    .HasForeignKey(c => c.ModelId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(c => c.Trim)
                    .WithMany(t => t.Cars)
                    .HasForeignKey(c => c.TrimId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Repair>()
                    .HasOne(r => r.Car)
                    .WithMany(c => c.Repairs)
                    .HasForeignKey(r => r.CarId)
                    .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<CarSale>()
                    .HasOne(s => s.Car)
                    .WithOne(c => c.Sale)
                    .HasForeignKey<CarSale>(s => s.CarId)
                    .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
