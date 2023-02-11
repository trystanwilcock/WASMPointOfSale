namespace WASMPointOfSale.Shared.ViewModels
{
    public class SaleDetailViewModel
    {
        public DateTime Timestamp { get; set; }

        public int Quantity { get; set; }

        public decimal Tax { get; set; }

        public decimal Total { get; set; }

        public decimal Discount { get; set; }

        public decimal Due { get; set; }


        public IEnumerable<SaleDetailProductViewModel>? Products { get; set; }

        public IEnumerable<SaleDetailTransactionViewModel>? Transactions { get; set; }
    }

    public class SaleDetailProductViewModel
    {
        public string Code { get; set; }

        public string Name { get; set; }

        public decimal Price { get; set; }

        public decimal Tax { get; set; }
    }

    public class SaleDetailTransactionViewModel
    {
        public string Type { get; set; }

        public decimal Amount { get; set; }
    }
}