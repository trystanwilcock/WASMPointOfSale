using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WASMPointOfSale.Server.Data;
using WASMPointOfSale.Server.Models;
using WASMPointOfSale.Shared.Classes;
using WASMPointOfSale.Shared.DTOs;

namespace WASMPointOfSale.Server.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class SaleController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<SaleController> _logger;

        public SaleController(ApplicationDbContext context,
            ILogger<SaleController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpPost]
        public async Task NewSale(NewSaleDTO newSaleDTO)
        {
            Sale newSale = GetNewSale(newSaleDTO);
            await _context.AddAsync(newSale);
            await _context.SaveChangesAsync();
        }

        private static Sale GetNewSale(NewSaleDTO newSaleDTO)
        {
            DateTime timestamp = DateTime.Now;

            Sale sale = new()
            {
                Timestamp = timestamp,
                Quantity = newSaleDTO.Cart.Quantity,
                Tax = newSaleDTO.Cart.Tax,
                Total = newSaleDTO.Cart.Total,
                Discount = newSaleDTO.Cart.DiscountAmount,
                Due = newSaleDTO.Cart.Due,
                SaleProducts = GetNewSaleProducts(newSaleDTO.Cart.Lines, timestamp),
                SaleTransactions = GetNewSaleTransactions(newSaleDTO.Payment.PaymentAmount, timestamp)
            };

            return sale;
        }

        private static List<SaleProduct> GetNewSaleProducts(List<CartLine> cartLines, DateTime timestamp)
        {
            return cartLines.Select(l => new SaleProduct
            {
                Timestamp = timestamp,
                Code = l.Product.Code,
                Name = l.Product.Name,
                Price = l.Product.Price,
                Tax = l.Product.Tax
            }).ToList();
        }

        private static List<SaleTransaction> GetNewSaleTransactions(decimal paymentAmount, DateTime timestamp)
        {
            return new List<SaleTransaction>
            {
                new SaleTransaction
                {
                    Timestamp = timestamp,
                    Type = SaleTransactionType.Payment.ToString(),
                    Amount = paymentAmount
                }
            };
        }
    }
}