using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WASMPointOfSale.Server.Data;
using WASMPointOfSale.Server.Models;
using WASMPointOfSale.Shared.Classes;
using WASMPointOfSale.Shared.DTOs;
using WASMPointOfSale.Shared.ViewModels;

namespace WASMPointOfSale.Server.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class SaleController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<SaleController> _logger;

        public SaleController(ApplicationDbContext context,
            IMapper mapper,
            ILogger<SaleController> logger)
        {
            _context = context;
            this._mapper = mapper;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IEnumerable<SaleViewModel>> Get()
        {
            return await _context
                .Sales
                .Include(s => s.SaleTransactions)
                .OrderByDescending(s => s.Timestamp)
                .Select(s => new SaleViewModel
                {
                    Id = s.Id,
                    Timestamp = s.Timestamp,
                    Quantity = s.Quantity,
                    Tax = s.Tax,
                    Total = s.Due,
                    Discount = s.Discount,
                    Due = s.Due,
                    Paid = s.SaleTransactions!.Where(st => st.Type == SaleTransactionType.Payment.ToString()).Sum(st => st.Amount)
                })
                .ToArrayAsync();
        }

        [HttpGet("details/{id:int}")]
        public async Task<SaleDetailViewModel> GetSaleDetails(int id)
        {
            return await _context
                .Sales
                .Where(s => s.Id == id)
                .Include(s => s.SaleProducts)
                .Include(s => s.SaleTransactions)
                .Select(s => new SaleDetailViewModel
                {
                    Timestamp = s.Timestamp,
                    Quantity = s.Quantity,
                    Tax = s.Tax,
                    Total = s.Total,
                    Discount = s.Discount,
                    Due = s.Due,
                    Paid = s.SaleTransactions!.Where(st => st.Type == SaleTransactionType.Payment.ToString()).Sum(st => st.Amount),
                    Products = _mapper.Map<IEnumerable<SaleDetailProductViewModel>>(s.SaleProducts),
                    Transactions = _mapper.Map<IEnumerable<SaleDetailTransactionViewModel>>(s.SaleTransactions)
                })
                .FirstAsync();
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
            List<SaleProduct> saleProducts = new();

            foreach (var cartLine in cartLines)
            {
                for (int i = 0; i < cartLine.Quantity; i++)
                {
                    saleProducts.Add(new SaleProduct
                    {
                        Timestamp = timestamp,
                        Code = cartLine.Product.Code,
                        Name = cartLine.Product.Name,
                        Price = cartLine.Product.Price,
                        Tax = cartLine.Product.Tax
                    });
                }
            }

            return saleProducts;
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

        [HttpPost]
        [Route("addpayment")]
        public async Task AddPaymentToSale(AddPaymentDTO addPaymentDTO)
        {
            SaleTransaction saleTransaction = new SaleTransaction
            {
                SaleId = addPaymentDTO.SaleId,
                Timestamp = DateTime.Now,
                Type = SaleTransactionType.Payment.ToString(),
                Amount = addPaymentDTO.PaymentAmount
            };
            await _context.AddAsync(saleTransaction);
            await _context.SaveChangesAsync();
        }
    }
}