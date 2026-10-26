using ExpressVoitures.Helpers;
using ExpressVoitures.Models.Entities;
using ExpressVoitures.Models.Services;
using ExpressVoitures.Models.Services.Interfaces;
using ExpressVoitures.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace ExpressVoitures.Controllers
{
    public class CarsController : GenericEntityController<Car, CarViewModel, ICarService>
    {
        private readonly string pictureFolderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "AppData", "LocalLow", "ExpressVoitures");
        public CarsController(ICarService carService) : base(carService)
        {
        }

        public override IActionResult Index()
        {
            return View(_service.GetViewModels());
        }

        [HttpGet]
        public IActionResult Details(int id)
        {
            return View("_CarDetails", _service.GetViewModel(id));
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

            return base.Create(viewModel);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public override IActionResult Delete(int id)
        {
            try
            {
                var pictureId = _service.GetViewModel(id)?.PictureId;

                _service.Delete(id);

                if (!string.IsNullOrEmpty(pictureId))
                {
                    var fullPath = Path.Combine(pictureFolderPath, pictureId);
                    if (System.IO.File.Exists(fullPath))
                    {
                        System.IO.File.Delete(fullPath);
                    }
                }

                TempData["Success"] = "La marque a été supprimée.";
            }
            catch (DbUpdateException)
            {
                // Erreur typique : contrainte FK (des modèles/voitures liées)
                TempData["Error"] = "Impossible de supprimer cette marque : des enregistrements liés existent.";
            }
            catch (Exception)
            {
                TempData["Error"] = "Une erreur est survenue lors de la suppression.";
            }

            return RedirectToAction(nameof(Create));
        }

        [HttpGet]
        public override IActionResult Edit(int id)
        {
            var vm = _service.GetViewModel(id);

            if (vm == null)
                return NotFound();

            _service.FillViewModel(vm);
            _service.FillEditViewData(vm, ViewData);

            return View(vm);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public override IActionResult Edit(CarViewModel viewModel)
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

                        var fileName = string.IsNullOrEmpty(viewModel.PictureId) ? $"{Guid.NewGuid()}{ext}" : viewModel.PictureId;
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
                _service.Update(viewModel);

                TempData["Success"] = "Voiture mise à jour.";
                return RedirectToAction(nameof(Details), new { id = viewModel.Id });
            }
            else
            {
                TempData["Error"] = string.Join("<br>",
                            ModelState.Values
                                .SelectMany(v => v.Errors)
                                .Select(e => e.ErrorMessage));

                return RedirectToAction(nameof(Edit), new { id = viewModel.Id });
            }
        }
    }
}
