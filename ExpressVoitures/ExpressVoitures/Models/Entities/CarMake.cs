namespace ExpressVoitures.Models.Entities
{
    public class CarMake
    {
        public int Id { get; set; }
        public string? Name { get; set; }



        public ICollection<CarModel> Models { get; set; } = new HashSet<CarModel>();
        public ICollection<CarTrim> Trims { get; set; } = new HashSet<CarTrim>();
        public ICollection<Car> Cars { get; set; } = new HashSet<Car>();
    }
}
