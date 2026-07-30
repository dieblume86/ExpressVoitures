using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;

namespace ExpressVoitures.Models.ViewModels
{
    public class CarViewModel
    {
        [BindNever]
        public int Id { get; set; }

        [Required(ErrorMessageResourceName = "MissingVinCode")]
        public string VinCode { get; set; }

        [Required(ErrorMessageResourceName = "MissingYear")]
        public int Year { get; set; }


        [Required(ErrorMessageResourceName = "MissingMakeId")]
        public int MakeId { get; set; }

         [Required(ErrorMessageResourceName = "MissingModelId")]
        public int ModelId { get; set; }

         [Required(ErrorMessageResourceName = "MissingTrimId")]
        public int TrimId { get; set; }
    }
}
