using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;

namespace ExpressVoitures.Models.ViewModels
{
    public class CarSaleViewModel
    {
        private const string _missing = "Missing";

        public int Id { get; set; }

        [Required(ErrorMessage = $"{_missing}{nameof(PurchaseDate)}")]
        public DateTimeOffset PurchaseDate { get; set; }

        [Required(ErrorMessage = $"{_missing}{nameof(PurchasePrice)}")]
        public float PurchasePrice { get; set; }

        public DateTimeOffset? AvailableForSaleDate { get; set; }

        public float SalePrice { get; set; }

        public DateTimeOffset? SaleDate { get; set; }


        [Required(ErrorMessage = $"{_missing}{nameof(CarId)}")]
        public int CarId { get; set; }


        [BindNever]
        public CarViewModel? Car { get; set; }
    }
}
