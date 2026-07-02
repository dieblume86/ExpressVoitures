namespace ExpressVoitures.Models.Entities
{
    public class Car
    {
        public Car()
        {
            Repairs = new HashSet<Repair>();
        }

        public int Id { get; set; }

        public string? VinCode { get; set; }

        public int Year { get; set; }

        public string? Make { get; set; }

        public string? Model { get; set; }

        public string? Trim { get; set; }

        public ICollection<Repair> Repairs { get; set; } = new List<Repair>();
    }
}
