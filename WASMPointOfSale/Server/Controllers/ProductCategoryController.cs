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
    public class ProductCategoryController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<ProductCategoryController> _logger;

        public ProductCategoryController(ApplicationDbContext context,
            IMapper mapper,
            ILogger<ProductCategoryController> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IEnumerable<ProductCategoryViewModel>> Get()
        {
            return await _context
                .ProductCategories
                .ProjectTo<ProductCategoryViewModel>(_mapper.ConfigurationProvider)
                .OrderBy(pc => pc.Name)
                .ToArrayAsync();
        }

        [HttpGet("getcreateupdatedto/{id:int}")]
        public async Task<CreateUpdateProductCategoryDTO> GetCreateUpdateDTO(int id)
        {
            return await _context
                .ProductCategories
                .Where(pc => pc.Id == id)
                .ProjectTo<CreateUpdateProductCategoryDTO>(_mapper.ConfigurationProvider)
                .FirstAsync();
        }

        [HttpPost]
        public async Task Create(CreateUpdateProductCategoryDTO productCategoryDTO)
        {
            await _context.AddAsync(_mapper.Map<ProductCategory>(productCategoryDTO));
            await _context.SaveChangesAsync();
        }

        [HttpPut]
        public async Task Update(CreateUpdateProductCategoryDTO productCategoryDTO)
        {
            ProductCategory productCategory = await _context
                .ProductCategories
                .FindAsync(productCategoryDTO.Id);
            _mapper.Map(productCategoryDTO, productCategory);
            _context.Update(productCategory);
            await _context.SaveChangesAsync();
        }

        [HttpDelete("{id:int}")]
        public async Task Delete(int id)
        {
            ProductCategory productCategory = await _context
                .ProductCategories
                .FindAsync(id);
            _context.Remove(productCategory);
            await _context.SaveChangesAsync();
        }
    }
}