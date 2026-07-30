using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;

namespace ExpressVoitures.Models.ViewModels
{
    public class CarModelViewModel
    {
        [BindNever]
        public int Id { get; set; }

        [Required(ErrorMessageResourceName = "MissingName")]
        public string Name { get; set; }

        [Required(ErrorMessageResourceName = "MissingMakeId")]
        public int MakeId { get; set; }
    }
}
