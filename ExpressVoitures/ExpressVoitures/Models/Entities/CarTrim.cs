using System.ComponentModel.DataAnnotations.Schema;

namespace ExpressVoitures.Models.Entities
{
    public class CarTrim
    {
        public int Id { get; set; }
        public string? Name { get; set; }

        public int ModelId { get; set; }


        public CarModel? Model { get; set; }
        public ICollection<Car> Cars { get; set; } = new HashSet<Car>();
    }
}
