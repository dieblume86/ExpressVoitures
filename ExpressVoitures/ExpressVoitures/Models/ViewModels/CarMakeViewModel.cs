using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;

namespace ExpressVoitures.Models.ViewModels
{
    public class CarMakeViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Un nom est requis")]
        public string Name { get; set; }
    }
}
