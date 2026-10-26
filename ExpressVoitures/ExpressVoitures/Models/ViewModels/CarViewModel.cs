using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;

namespace ExpressVoitures.Models.ViewModels
{
    public class CarViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Un code VIN est requis.")]
        public string VinCode { get; set; }

        [Required(ErrorMessage = "Une année est requise.")]
        public int Year { get; set; }

        public string? PictureId { get; set; }
        public IFormFile? PictureFile { get; set; }

        [Required(ErrorMessage = "Une finition est requise.")]
        public int TrimId { get; set; }


        [BindNever]
        public CarTrimViewModel? Trim { get; set; }
        [BindNever]
        public CarSaleViewModel? Sale { get; set; }
        [BindNever]
        public ICollection<RepairViewModel> Repairs { get; set; } = new HashSet<RepairViewModel>();
    }
}
