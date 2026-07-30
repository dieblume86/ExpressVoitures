using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;

namespace ExpressVoitures.Models.ViewModels
{
    public class RepairViewModel
    {
        [BindNever]
        public int Id { get; set; }

        [Required(ErrorMessageResourceName = "MissingDescription")]
        public string Description { get; set; }

        [Required(ErrorMessageResourceName = "MissingRepairCost")]
        public float RepairCost { get; set; }

        [Required(ErrorMessageResourceName = "MissingCarId")]
        public int CarId { get; set; }
    }
}
