namespace WASMPointOfSale.Shared.ViewModels
{
    public class StockViewModel
    {
        public int ProductId { get; set; }

        public string ProductName { get; set; } = default!;

        public int StockAdded { get; set; }

        public int ItemsSold { get; set; }

        public int StockRemaining
        {
            get
            {
                return StockAdded - ItemsSold;
            }
        }
    }
}
