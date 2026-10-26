using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;

namespace ExpressVoitures.Models.ViewModels
{
    public class CarTrimViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Un nom est requis.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Un modèle est requis.")]
        public int ModelId { get; set; }


        [BindNever]
        public CarModelViewModel? Model { get; set; }
    }
}
