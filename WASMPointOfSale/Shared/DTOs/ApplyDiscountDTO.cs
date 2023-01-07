using System.ComponentModel.DataAnnotations;

namespace WASMPointOfSale.Shared.DTOs
{
    public class ApplyDiscountDTO
    {
        [Required]
        [Range(1, 99)]
        [Display(Name = "Discount Amount")]
        public decimal DiscountAmount { get; set; }
    }
}
