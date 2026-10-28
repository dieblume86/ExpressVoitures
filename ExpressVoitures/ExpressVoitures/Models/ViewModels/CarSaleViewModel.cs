using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;

namespace ExpressVoitures.Models.ViewModels
{
    public class CarSaleViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Date d'achat requis")]
        public DateTimeOffset PurchaseDate { get; set; }

        [Required(ErrorMessage = "Prix d'achat requis")]
        public float PurchasePrice { get; set; }

        public DateTimeOffset? AvailableForSaleDate { get; set; }

        public float SalePrice { get; set; }

        public DateTimeOffset? SaleDate { get; set; }


        [Required(ErrorMessage = "Identifiant de la voiture requis")]
        public int CarId { get; set; }


        [BindNever]
        public CarViewModel? Car { get; set; }
    }
}
