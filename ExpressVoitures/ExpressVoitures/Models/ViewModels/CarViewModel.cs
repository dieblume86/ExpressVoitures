using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;

namespace ExpressVoitures.Models.ViewModels
{
    public class CarViewModel
    {
        [BindNever]
        public int Id { get; set; }

        [Required(ErrorMessage = "MissingVinCode")]
        public string VinCode { get; set; }

        [Required(ErrorMessage = "MissingYear")]
        public int Year { get; set; }


        [Required(ErrorMessage = "MissingMakeId")]
        public int MakeId { get; set; }

         [Required(ErrorMessage = "MissingModelId")]
        public int ModelId { get; set; }

         [Required(ErrorMessage = "MissingTrimId")]
        public int TrimId { get; set; }
    }
}
