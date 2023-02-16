namespace WASMPointOfSale.Server.Models
{
    public class SaleProduct
    {
        public int Id { get; set; }

        public int SaleId { get; set; }

        public DateTime Timestamp { get; set; }

        public string? Code { get; set; }

        public string? Name { get; set; }

        public decimal Price { get; set; }

        public decimal Tax { get; set; }


        public Sale? Sale { get; set; }
    }
}
