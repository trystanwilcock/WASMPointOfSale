namespace WASMPointOfSale.Server.Models
{
    public class Sale
    {
        public int Id { get; set; }

        public DateTime Timestamp { get; set; }

        public int Quantity { get; set; }

        public decimal Tax { get; set; }

        public decimal Total { get; set; }

        public decimal Discount { get; set; }

        public decimal Due { get; set; }


        public List<SaleProduct>? SaleProducts { get; set; }

        public List<SaleTransaction>? SaleTransactions { get; set; }
    }
}
