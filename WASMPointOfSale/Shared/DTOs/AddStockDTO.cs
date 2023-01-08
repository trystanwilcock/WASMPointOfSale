using System.ComponentModel.DataAnnotations;

namespace WASMPointOfSale.Shared.DTOs
{
    public class AddStockDTO
    {
        public int ProductId { get; set; }

        [Range(1, double.MaxValue, ErrorMessage = "Quantity must be at least 1.")]
        public int Quantity { get; set; }
    }
}
