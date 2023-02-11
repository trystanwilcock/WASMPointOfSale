namespace WASMPointOfSale.Shared.ViewModels
{
    public class SaleViewModel
    {
        public int Id { get; set; }

        public DateTime Timestamp { get; set; }

        public int Quantity { get; set; }

        public decimal TotalDue { get; set; }

        public decimal TotalPaid { get; set; }
    }
}