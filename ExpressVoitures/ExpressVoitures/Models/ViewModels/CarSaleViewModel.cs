using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;

namespace ExpressVoitures.Models.ViewModels
{
    public class CarSaleViewModel
    {
        private const string _missingName = "MissingName";

        [BindNever]
        public int Id { get; set; }

        [Required(ErrorMessageResourceName = $"{_missingName}{nameof(PurchaseDate)}")]
        public DateTimeOffset PurchaseDate { get; set; }
        [Required(ErrorMessageResourceName = $"{_missingName}{nameof(PurchasePrice)}")]
        public float PurchasePrice { get; set; }
        public DateTimeOffset? AvailableForSaleDate { get; set; }

        [Required(ErrorMessageResourceName = $"{_missingName}{nameof(SalePrice)}")]
        public float SalePrice { get; set; }
        public DateTimeOffset? SaleDate { get; set; }


        [Required(ErrorMessageResourceName = $"{_missingName}{nameof(CarId)}")]
        public int CarId { get; set; }
    }
}
