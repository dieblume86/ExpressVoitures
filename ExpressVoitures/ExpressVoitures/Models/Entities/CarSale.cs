namespace ExpressVoitures.Models.Entities
{
    public class CarSale
    {
        public int Id { get; set; }

        public int CarId { get; set; }

        public DateTime PurchaseDate { get; set; }

        public float PurchasePrice { get; set; }

        public DateTime? AvailableForSaleDate { get; set; }

        public float SalePrice { get; set; }

        public DateTime? SaleDate { get; set; }
    }
}
