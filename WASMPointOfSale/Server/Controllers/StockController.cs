using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WASMPointOfSale.Server.Data;
using WASMPointOfSale.Server.Models;
using WASMPointOfSale.Shared.DTOs;
using WASMPointOfSale.Shared.ViewModels;

namespace WASMPointOfSale.Server.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class StockController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<StockController> _logger;

        public StockController(ApplicationDbContext context,
            IMapper mapper,
            ILogger<StockController> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IEnumerable<StockViewModel>> Get()
        {
            var stockRecords = await (from p in _context.Products
                          select new StockViewModel
                          {
                              ProductId = p.Id,
                              ProductName = p.Name,
                                          StockAdded = (
                                            (from s in _context.Stocks
                                             where s.ProductId == p.Id
                                             select s.Quantity).Sum()
                                          ),
                                          ItemsSold = (
                                            (from s in _context.SaleProducts
                                             where s.Code == p.Code
                                             select s).Count()
                                          )
                          })
                          .ToArrayAsync();

            return stockRecords
                .OrderBy(vm => vm.StockRemaining)
                .ThenBy(vm => vm.ProductName);
        }

        [HttpPost]
        public async Task Add(AddStockDTO addStockDTO)
        {
            await _context.AddAsync(_mapper.Map<Stock>(addStockDTO));
            await _context.SaveChangesAsync();
        }
    }
}