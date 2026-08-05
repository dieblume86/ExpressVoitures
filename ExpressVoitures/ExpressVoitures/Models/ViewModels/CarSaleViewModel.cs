using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;

namespace ExpressVoitures.Models.ViewModels
{
    public class CarSaleViewModel
    {
        private const string _missing = "Missing";

        [BindNever]
        public int Id { get; set; }

        [Required(ErrorMessageResourceName = $"{_missing}{nameof(PurchaseDate)}")]
        public DateTimeOffset PurchaseDate { get; set; }

        [Required(ErrorMessageResourceName = $"{_missing}{nameof(PurchasePrice)}")]
        public float PurchasePrice { get; set; }

        public DateTimeOffset? AvailableForSaleDate { get; set; }

        [Required(ErrorMessageResourceName = $"{_missing}{nameof(SalePrice)}")]
        public float SalePrice { get; set; }

        public DateTimeOffset? SaleDate { get; set; }


        [Required(ErrorMessageResourceName = $"{_missing}{nameof(CarId)}")]
        public int CarId { get; set; }
    }
}
