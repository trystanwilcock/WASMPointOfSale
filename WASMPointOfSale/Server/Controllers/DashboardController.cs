using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WASMPointOfSale.Server.Data;
using WASMPointOfSale.Shared.ViewModels;

namespace WASMPointOfSale.Server.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class DashboardController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<SaleController> _logger;
        private readonly StockController _stockController;

        public DashboardController(ApplicationDbContext context,
            ILogger<SaleController> logger,
            StockController stockController)
        {
            _context = context;
            _logger = logger;
            _stockController = stockController;
        }

        [HttpGet]
        public async Task<DashboardViewModel> Get()
        {
            var totalSales = _context
                .Sales
                .Sum(s => s.Due);

            var stockRecords = await _stockController.Get();
            var criticalStockRecords = stockRecords
                .Where(s => s.StockLevelCritical);

            var totalItems = _context
                .Products
                .Count();

            var totalItemsSold = _context
                .SaleProducts
                .Select(p => p.Code)
                .Distinct()
                .Count();

            return new DashboardViewModel
            {
                TotalSales = totalSales,
                CriticalStock = criticalStockRecords.Count(),
                TotalItems = totalItems,
                TotalItemsSold = totalItemsSold
            };
        }
    }
}