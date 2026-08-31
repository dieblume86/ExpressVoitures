using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;

namespace ExpressVoitures.Models.ViewModels
{
    public class CarModelViewModel
    {
        [BindNever]
        public int Id { get; set; }

        [Required(ErrorMessage = "MissingName")]
        public string Name { get; set; }

        [Required(ErrorMessage = "MissingMakeId")]
        public int MakeId { get; set; }


        [BindNever]
        public CarMakeViewModel? CarMakeViewModel { get; set; }
    }
}
