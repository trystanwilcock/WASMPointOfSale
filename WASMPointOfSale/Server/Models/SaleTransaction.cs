namespace WASMPointOfSale.Server.Models
{
    public class SaleTransaction
    {
        public int Id { get; set; }

        public int SaleId { get; set; }

        public DateTime Timestamp { get; set; }

        public string Type { get; set; }

        public decimal Amount { get; set; }


        public Sale Sale { get; set; }
    }
}
