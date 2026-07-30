namespace ExpressVoitures.Models.Entities
{
    public class CarModel
    {
        public int Id { get; set; }
        public string? Name { get; set; }


        public int MakeId { get; set; }


        public CarMake? Make { get; set; }
        public ICollection<CarTrim> Trims { get; set; } = new HashSet<CarTrim>();
        public ICollection<Car> Cars { get; set; } = new HashSet<Car>();
    }
}
