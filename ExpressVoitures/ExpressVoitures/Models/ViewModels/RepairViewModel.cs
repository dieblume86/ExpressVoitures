using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;

namespace ExpressVoitures.Models.ViewModels
{
    public class RepairViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Description requise")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Coût de la réparation requis")]
        public float RepairCost { get; set; }

        [Required(ErrorMessage = "Identifiant de la voiture requis")]
        public int CarId { get; set; }


        [BindNever]
        public CarViewModel? Car { get; set; }
    }
}
