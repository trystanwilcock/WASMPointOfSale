namespace WASMPointOfSale.Server.Models
{
    public class Product
    {
        public int Id { get; set; }

        public int ProductCategoryId { get; set; }

        public string? Code { get; set; }

        public string? Name { get; set; }

        public string? Description { get; set; }

        public decimal Price { get; set; }

        public decimal Tax { get; set; }

        public int ReorderAtStockLevel { get; set; }


        public ProductCategory? ProductCategory { get; set; }
    }
}
