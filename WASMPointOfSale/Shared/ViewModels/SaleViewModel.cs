namespace WASMPointOfSale.Shared.ViewModels
{
    public class SaleViewModel
    {
        public DateTime Timestamp { get; set; }

        public int Quantity { get; set; }

        public decimal TotalDue { get; set; }

        public decimal TotalPaid { get; set; }
    }
}