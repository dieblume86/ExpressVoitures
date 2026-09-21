using ExpressVoitures.Helpers;
using ExpressVoitures.Models.Entities;
using ExpressVoitures.Models.Services.Interfaces;
using ExpressVoitures.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.IO;

namespace ExpressVoitures.Controllers
{
    public class CarsController : GenericEntityController<Car, CarViewModel, ICarService>
    {
        private readonly ICarMakeService _carMakeService;
        private const string unknownMake = "Marque inconnue";

        private readonly ICarModelService _carModelService;
        private const string unknownModel = "Modèle inconnu";

        private readonly ICarTrimService _carTrimService;
        private const string unknownTrim = "Finition inconnue";

        private readonly string pictureFolderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "AppData", "LocalLow", "ExpressVoitures");
        //C:\Users\Megaport\AppData\LocalLow\ExpressVoitures\185d3bed-0b25-4995-8ce8-68454bf50882.png

        public CarsController(ICarService carService, ICarMakeService carMakeService, ICarModelService carModelService, ICarTrimService carTrimService) : base(carService)
        {
            _carMakeService = carMakeService;
            _carModelService = carModelService;
            _carTrimService = carTrimService;
        }

        public override IActionResult Index()
        {
            return View(GetCars());
        }

        [HttpGet]
        public IActionResult Details(int id)
        {
            return View("_CarDetails", GetCar(id));
        }
        [HttpGet]
        public IActionResult GetPicture(string pictureId)
        {
            if (string.IsNullOrEmpty(pictureId))
                return NotFound();

            var path = Path.Combine(pictureFolderPath, pictureId);
            if (!System.IO.File.Exists(path))
                return NotFound();

            var allowedFolder = System.IO.Path.GetFullPath(pictureFolderPath);
            var fullPath = System.IO.Path.GetFullPath(path);
            if (!fullPath.StartsWith(allowedFolder, StringComparison.OrdinalIgnoreCase))
            {
                return BadRequest();
            }

            var provider = new Microsoft.AspNetCore.StaticFiles.FileExtensionContentTypeProvider();
            if (!provider.TryGetContentType(fullPath, out var contentType))
            {
                contentType = "application/octet-stream";
            }

            var stream = System.IO.File.OpenRead(fullPath);
            return File(stream, contentType);
        }

        [Authorize]
        [HttpPost]
        public override IActionResult Create(CarViewModel viewModel)
        {
            if (viewModel.PictureFile != null && viewModel.PictureFile.Length > 0)
            {
                var allowed = new[] { ".jpg", ".jpeg", ".png", ".gif" };
                var ext = Path.GetExtension(viewModel.PictureFile.FileName)?.ToLowerInvariant();
                if (string.IsNullOrEmpty(ext) || !allowed.Contains(ext))
                {
                    ModelState.AddModelError("PhotoFile", "Type de fichier non autorisé. Utilisez jpg/png/gif.");
                }
                else if (viewModel.PictureFile.Length > 5 * 1024 * 1024)
                {
                    ModelState.AddModelError("PhotoFile", "Fichier trop volumineux (max 5MB).");
                }
                else
                {
                    try
                    {
                        var userProfile = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);

                        if (!Directory.Exists(pictureFolderPath))
                            Directory.CreateDirectory(pictureFolderPath);

                        var fileName = $"{Guid.NewGuid()}{ext}";
                        var fullPath = Path.Combine(pictureFolderPath, fileName);
                        using (var stream = new FileStream(fullPath, FileMode.Create))
                        {
                            viewModel.PictureFile.CopyTo(stream);
                        }

                        viewModel.PictureId = fileName;
                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError("PhotoFile", "Impossible d'enregistrer l'image : " + ex.Message);
                    }
                }
            }

            IEnumerable<string> modelErrors = _service.CheckModelErrors(viewModel);

            foreach (string error in modelErrors)
            {
                ModelState.AddModelError("", error);
            }

            if (ModelState.IsValid)
            {
                _service.Add(viewModel);

                TempData["Success"] = "Success.";
                return RedirectToAction(nameof(Details), viewModel);
            }
            else
            {
                TempData["Error"] = "Une erreur est survenue.";
                return RedirectToAction(nameof(Create));
            }
        }


        [HttpGet]
        public override IActionResult Edit(int id)
        {
            var vm = _service.GetViewModel(id);

            if (vm == null)
                return NotFound();

            FillCarViewModel(vm);

            var selectedMakeId = vm.Trim?.Model?.MakeId ?? 0;
            var makes = _carMakeService.GetViewModels().OrderBy(m => m.Name).ToList();
            ViewData[ViewDataKeys.Makes] = new SelectList(makes, "Id", "Name", selectedMakeId);

            var selectedModelId = vm.Trim?.ModelId ?? 0;
            var models = _carModelService.GetViewModels()
                .Where(m => m.MakeId == selectedMakeId)
                .OrderBy(m => m.Name)
                .ToList();
            ViewData[ViewDataKeys.Models] = new SelectList(models, "Id", "Name", vm.Trim.ModelId);

            var selectedTrimId = vm.Trim?.Model?.MakeId ?? 0;
            var trims = _carTrimService.GetViewModels()
                .Where(m => m.ModelId == vm.Trim.ModelId)
                .OrderBy(m => m.Name)
                .ToList();
            ViewData[ViewDataKeys.Trims] = new SelectList(trims, "Id", "Name", vm.TrimId);

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public override IActionResult Edit(CarViewModel vm)
        {
            var selectedMakeId = vm.Trim?.Model?.MakeId ?? 0;
            var makes = _carMakeService.GetViewModels().OrderBy(m => m.Name).ToList();
            ViewData[ViewDataKeys.Makes] = new SelectList(makes, "Id", "Name", selectedMakeId);

            var selectedModelId = vm.Trim?.ModelId ?? 0;
            var models = _carModelService.GetViewModels()
                .Where(m => m.MakeId == selectedMakeId)
                .OrderBy(m => m.Name)
                .ToList();
            ViewData[ViewDataKeys.Models] = new SelectList(models, "Id", "Name", vm.Trim.ModelId);

            var selectedTrimId = vm.Trim?.Model?.MakeId ?? 0;
            var trims = _carTrimService.GetViewModels()
                .Where(m => m.ModelId == vm.Trim.ModelId)
                .OrderBy(m => m.Name)
                .ToList();
            ViewData[ViewDataKeys.Trims] = new SelectList(trims, "Id", "Name", vm.TrimId);

            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            _service.Update(vm);
            TempData["Success"] = "Finition mise à jour.";
            return RedirectToAction(nameof(Create));
        }


        protected override void SetViewDatas()
        {
            var makes = _carMakeService.GetViewModels().OrderBy(m => m.Name);
            ViewData[ViewDataKeys.Makes] = new SelectList(makes, "Id", "Name");

            var models = _carModelService.GetViewModels().OrderBy(m => m.Name);
            ViewData[ViewDataKeys.Models] = new SelectList(Enumerable.Empty<object>(), "Id", "Name");

            var trims = _carTrimService.GetViewModels().OrderBy(m => m.Name);
            ViewData[ViewDataKeys.Trims] = new SelectList(Enumerable.Empty<object>(), "Id", "Name");

            ViewData[dataExistingItems] = GetCars().OrderBy(m => m.Trim?.Model?.Make?.Name)
                .ThenBy(m => m.Trim?.Model?.Name)
                .ThenBy(m => m.Trim?.Name)
                .ToList();
        }
        private List<CarViewModel> GetCars()
        {
            var cars = _service.GetViewModels().ToList();

            foreach (var car in cars)
            {
                FillCarViewModel(car);
            }

            return cars;
        }
        private CarViewModel GetCar(int id)
        {
            var vm = _service.GetViewModel(id);

            if (vm == null)
                return new CarViewModel();

            FillCarViewModel(vm);

            return vm;
        }
        private void FillCarViewModel(CarViewModel vm)
        {
            if (vm == null)
                return;

            vm.Trim = _carTrimService.GetViewModel(vm.TrimId);
            if (vm.Trim != null)
            {
                vm.Trim.Model = _carModelService.GetViewModel(vm.Trim.ModelId);
                if (vm.Trim.Model != null)
                {
                    vm.Trim.Model.Make = _carMakeService.GetViewModel(vm.Trim.Model.MakeId);
                }
            }
        }
    }
}
