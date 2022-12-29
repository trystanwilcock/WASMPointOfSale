using AutoMapper;
using Microsoft.AspNetCore.Routing.Constraints;
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
            CreateMap<ProductCategory, SelectListItemDTO>()
                .ForMember(dest => dest.Text, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Value, opt => opt.MapFrom(src => src.Id));

            CreateMap<Product, ProductViewModel>();
            CreateMap<Product, CreateUpdateProductDTO>().ReverseMap();
        }
    }
}