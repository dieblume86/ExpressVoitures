namespace ExpressVoitures.Models.Entities
{
    public class CarSale
    {
        public int Id { get; set; }
        public DateTimeOffset PurchaseDate { get; set; }
        public float PurchasePrice { get; set; }
        public DateTimeOffset? AvailableForSaleDate { get; set; }
        public float SalePrice { get; set; }
        public DateTimeOffset? SaleDate { get; set; }



        public int CarId { get; set; }


        public Car? Car { get; set; }
    }
}
