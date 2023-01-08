using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WASMPointOfSale.Server.Data;
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
            return await (from p in _context.Products
                          select new StockViewModel
                          {
                              ProductId = p.Id,
                              ProductName = p.Name,
                              StockLevel = _context.Stocks.Where(s => s.ProductId == p.Id).Sum(s => s.Quantity)
                          })
                          .OrderBy(vm => vm.StockLevel)
                          .ThenBy(vm => vm.ProductName)
                          .ToArrayAsync();
        }
    }
}