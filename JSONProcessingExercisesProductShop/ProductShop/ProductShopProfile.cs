namespace ProductShop
{
    using AutoMapper;
    using ProductShop.DTOs.Export;
    using ProductShop.DTOs.Import;
    using ProductShop.Models;

    public class ProductShopProfile : Profile
    {
        public ProductShopProfile() 
        {
            //User
            this.CreateMap<ImportUserDto, User>();

            //Product
            this.CreateMap<ImportProductDto, Product>();

            this.CreateMap<Product, ExportProductInRange>()
                .ForMember(dest => dest.ProductName,
                    opt => opt.MapFrom(s => s.Name))
                .ForMember(dest => dest.ProductPrice, 
                    opt => opt.MapFrom(s => s.Price))
                .ForMember(dest => dest.SellerName,
                    opt => opt.MapFrom(s => $"{s.Seller.FirstName} {s.Seller.LastName}"));

            //Category
            this.CreateMap<ImportCategoryDto, Category>();

            //CategoryProduct
            this.CreateMap<ImportCategoryProductDto, CategoryProduct>();
        }
    }
}
