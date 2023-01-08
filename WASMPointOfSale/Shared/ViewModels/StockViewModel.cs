namespace WASMPointOfSale.Shared.ViewModels
{
    public class StockViewModel
    {
        public int ProductId { get; set; }

        public string ProductName { get; set; } = default!;

        public int StockLevel { get; set; }
    }
}
