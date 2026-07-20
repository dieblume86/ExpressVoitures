using System.ComponentModel.DataAnnotations.Schema;

namespace ExpressVoitures.Models.Entities
{
    public class CarTrim
    {
        public int Id { get; set; }
        public string? Name { get; set; }



        public ICollection<CarMake> Makes { get; set; } = new HashSet<CarMake>();
        public ICollection<Car> Cars { get; set; } = new HashSet<Car>();
    }
}
