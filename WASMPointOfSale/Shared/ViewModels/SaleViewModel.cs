namespace WASMPointOfSale.Shared.ViewModels
{
    public class SaleViewModel
    {
        public int Id { get; set; }

        public DateTime Timestamp { get; set; }

        public int Quantity { get; set; }

        public decimal Tax { get; set; }

        public decimal Total { get; set; }

        public decimal Discount { get; set; }

        public decimal Due { get; set; }

        public decimal Paid { get; set; }

        public decimal Outstanding
        {
            get
            {
                return Due - Paid;
            }
        }
    }
}