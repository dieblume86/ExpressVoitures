using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;

namespace ExpressVoitures.Models.ViewModels
{
    public class RepairViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "MissingDescription")]
        public string Description { get; set; }

        [Required(ErrorMessage = "MissingRepairCost")]
        public float RepairCost { get; set; }

        [Required(ErrorMessage = "MissingCarId")]
        public int CarId { get; set; }


        [BindNever]
        public CarViewModel? Car { get; set; }
    }
}
