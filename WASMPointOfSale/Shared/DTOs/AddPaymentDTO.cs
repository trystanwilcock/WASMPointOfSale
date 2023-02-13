using System.ComponentModel.DataAnnotations;

namespace WASMPointOfSale.Shared.DTOs
{
    public class AddPaymentDTO
    {
        public AddPaymentDTO(decimal amountDue)
        {
            AmountDue = amountDue;
        }

        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Amount cannot be below 0.")]
        [Display(Name = "Payment Amount")]
        public decimal PaymentAmount { get; set; }

        public decimal AmountDue { get; set; }

        public decimal LeftToPay
        {
            get
            {
                return AmountDue - PaymentAmount;
            }
        }

        public int SaleId { get; set; }
    }
}
