using AutoMapper;
using ClothesApiAuthRepositoryUOW.Core.Dtos;
using ClothesApiAuthRepositoryUOW.Core.Models;

namespace ClothesApiAuthRepositoryUOW.Api.MappingData
{
    public class MappingProfile : Profile
    {
       
            public MappingProfile()
            {
                CreateMap<ApplicationUser, RegisterModelDto>().ReverseMap();

                CreateMap<Category, CategoryFormDto>().ReverseMap();

                CreateMap<Core.Models.Type , TypeFormDto>().ReverseMap();

                CreateMap<Category, CategoryDisplayDto>();

                CreateMap<Core.Models.Type, TypeDisplayDto>();


            CreateMap<Category, CatigoryTypesDisplayDto>().
                ForMember(dest => dest.CategoryName, src => src.MapFrom(src => src.Name))
                .ForMember(dest => dest.CategoryId, src => src.MapFrom(src => src.Id));


            CreateMap<Product, ProductFormDto>().ReverseMap()
                .ForMember(src => src.Images, opt => opt.Ignore())
                .ForMember(src => src.sizes, opt => opt.Ignore())
                .ForMember(src => src.colors, opt => opt.Ignore());


            CreateMap<Product, ProductDisplayDto>()
                .ForMember(dest => dest.CategoryName, src => src.MapFrom(src => src.Category.Name))
                .ForMember(dest => dest.TypeName, src => src.MapFrom(src => src.Type.Name))
                .ForMember(src => src.Images, opt => opt.Ignore());

            CreateMap<Product_Color_Size_Dto, Product_Color_Size>().ReverseMap();

            CreateMap<ProductImage, ImageDisplayDto>();

            }
        
    }
}
