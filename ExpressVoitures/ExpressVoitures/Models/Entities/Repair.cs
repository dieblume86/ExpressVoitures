namespace ExpressVoitures.Models.Entities
{
    public class Repair
    {
        public int Id { get; set; }
        public string? Description { get; set; }
        public float? RepairCost { get; set; }



        public int CarId { get; set; }


        public Car? Car { get; set; }
    }
}
