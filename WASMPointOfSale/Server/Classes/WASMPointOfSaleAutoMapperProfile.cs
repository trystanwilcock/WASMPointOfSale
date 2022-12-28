using AutoMapper;
using WASMPointOfSale.Server.Models;
using WASMPointOfSale.Shared.DTOs;
using WASMPointOfSale.Shared.ViewModels;

namespace WASMPointOfSale.Server.Classes
{
    public class WASMPointOfSaleAutoMapperProfile : Profile
    {
        public WASMPointOfSaleAutoMapperProfile()
        {
            CreateMap<ProductCategory, ProductCategoryViewModel>();
            CreateMap<ProductCategory, CreateUpdateProductCategoryDTO>().ReverseMap();
        }
    }
}