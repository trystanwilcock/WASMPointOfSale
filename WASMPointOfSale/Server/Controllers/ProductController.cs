using AutoMapper;
using AutoMapper.QueryableExtensions;
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
    public class ProductController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<ProductController> _logger;

        public ProductController(ApplicationDbContext context,
            IMapper mapper,
            ILogger<ProductController> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IEnumerable<ProductViewModel>> Get()
        {
            return await _context
                .Products
                .ProjectTo<ProductViewModel>(_mapper.ConfigurationProvider)
                .OrderBy(pc => pc.Name)
                .ToArrayAsync();
        }

        [HttpGet("getcreateupdatedto/{id:int}")]
        public async Task<CreateUpdateProductDTO> GetCreateUpdateDTO(int id)
        {
            return await _context
                .Products
                .Where(pc => pc.Id == id)
                .ProjectTo<CreateUpdateProductDTO>(_mapper.ConfigurationProvider)
                .FirstAsync();
        }

        [HttpPost]
        public async Task Create(CreateUpdateProductDTO productDTO)
        {
            await _context.AddAsync(_mapper.Map<Product>(productDTO));
            await _context.SaveChangesAsync();
        }

        [HttpPut]
        public async Task Update(CreateUpdateProductDTO productDTO)
        {
            Product product = await _context
                .Products
                .FindAsync(productDTO.Id);
            _mapper.Map(productDTO, product);
            _context.Update(product);
            await _context.SaveChangesAsync();
        }

        [HttpDelete("{id:int}")]
        public async Task Delete(int id)
        {
            Product product = await _context
                .Products
                .FindAsync(id);
            _context.Remove(product);
            await _context.SaveChangesAsync();
        }
    }
}