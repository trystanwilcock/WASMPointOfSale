using WASMPointOfSale.Shared.Classes;

namespace WASMPointOfSale.Shared.DTOs
{
    public class NewSaleDTO
    {
        public Cart Cart { get; set; }

        public AddPaymentDTO Payment { get; set; }
    }
}