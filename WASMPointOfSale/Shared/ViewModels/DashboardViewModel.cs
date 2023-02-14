namespace WASMPointOfSale.Shared.ViewModels
{
    public class DashboardViewModel
    {
        public decimal TotalSales { get; set; }

        public int CriticalStock { get; set; }

        public int TotalItems { get; set; }

        public int TotalItemsSold { get; set; }
    }
}
