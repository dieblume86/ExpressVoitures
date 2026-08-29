using ExpressVoitures.Models.Entities;
using ExpressVoitures.Models.ViewModels;
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
                .WithOne(model => model.Make)
                .HasForeignKey(model => model.MakeId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<CarModel>()
                .HasMany(model => model.Trims)
                .WithOne(trim => trim.Model)
                .HasForeignKey(trim => trim.ModelId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<CarTrim>()
               .HasOne(t => t.Model)
               .WithMany(m => m.Trims)
               .HasForeignKey(t => t.ModelId)
               .OnDelete(DeleteBehavior.Cascade);

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


            //modelBuilder.Entity<LoginModel>().HasData(IdentitySeedData);
        }
    }
}
