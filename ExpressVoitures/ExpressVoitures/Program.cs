using ExpressVoitures.Data;
using ExpressVoitures.Models.Profiles;
using ExpressVoitures.Models.Repositories;
using ExpressVoitures.Models.Repositories.Interfaces;
using ExpressVoitures.Models.Services;
using ExpressVoitures.Models.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddControllersWithViews();

builder.Services.AddTransient<ICarMakeRepository, CarMakeRepository>();
builder.Services.AddTransient<ICarMakeService, CarMakeService>();

builder.Services.AddTransient<ICarModelRepository, CarModelRepository>();
builder.Services.AddTransient<ICarModelService, CarModelService>();

builder.Services.AddTransient<ICarTrimRepository, CarTrimRepository>();
builder.Services.AddTransient<ICarTrimService, CarTrimService>();

builder.Services.AddTransient<ICarRepository, CarRepository>();
builder.Services.AddTransient<ICarService, CarService>();


builder.Services.AddAutoMapper(cfg =>
{
    cfg.AddProfile<CarMakeProfile>();
    cfg.AddProfile<CarModelProfile>();
    cfg.AddProfile<CarTrimProfile>();
    cfg.AddProfile<CarProfile>();
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Cars}/{action=Index}/{id?}")
    .WithStaticAssets();

app.MapRazorPages()
   .WithStaticAssets();

await IdentitySeedData.EnsurePopulated(app);

app.Run();
