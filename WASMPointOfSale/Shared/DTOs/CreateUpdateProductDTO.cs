using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WASMPointOfSale.Shared.DTOs
{
    public class CreateUpdateProductDTO
    {
        public int Id { get; set; }

        public int ProductCategoryId { get; set; }

        [Required]
        [StringLength(20)]
        public string Code { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [Required]
        [StringLength(200)]
        public string Description { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Price must be 0 or greater.")]
        public decimal Price { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Reorder stock level must be 0 or greater.")]
        public int ReorderAtStockLevel { get; set; }
    }
}