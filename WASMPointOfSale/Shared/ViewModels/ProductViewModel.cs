namespace WASMPointOfSale.Shared.ViewModels
{
    public class ProductViewModel
    {
        public int Id { get; set; }

        public int ProductCategoryId { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }

        public decimal Price { get; set; }

        public decimal Tax { get; set; }
    }
}