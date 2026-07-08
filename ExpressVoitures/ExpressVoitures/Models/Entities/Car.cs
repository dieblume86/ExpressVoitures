namespace ExpressVoitures.Models.Entities
{
    public class Car
    {
        public int Id { get; set; }
        public string? VinCode { get; set; }
        public int Year { get; set; }



        public int? MakeId { get; set; }
        public int? ModelId { get; set; }
        public int? TrimId { get; set; }


        public CarMake? Make { get; set; }
        public CarModel? Model { get; set; }
        public CarTrim? Trim { get; set; }
        public CarSale? Sale { get; set; }
        public ICollection<Repair> Repairs { get; set; } = new HashSet<Repair>();
    }
}
