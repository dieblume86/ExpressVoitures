namespace ExpressVoitures.Models.Entities
{
    public class Car
    {
        public int Id { get; set; }
        public string? VinCode { get; set; }
        public int Year { get; set; }
        public string? PictureId { get; set; }

         
        public int? TrimId { get; set; }


        public CarTrim? Trim { get; set; }
        public CarSale? Sale { get; set; }
        public ICollection<Repair> Repairs { get; set; } = new HashSet<Repair>();
    }
}
