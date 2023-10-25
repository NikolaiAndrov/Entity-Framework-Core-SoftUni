namespace ProductShop
{
    using AutoMapper;
    using DTOs.Import;
    using Models;
    using ProductShop.Data;
    using ProductShop.DTOs.Export;

    public class ProductShopProfile : Profile
    {
        public ProductShopProfile()
        {
            //User
            this.CreateMap<ImportUserDto, User>();

            this.CreateMap<User, ExportUserWithSoldProductDto>()
                .ForMember(d => d.Products, 
                opt => opt.MapFrom(s => s.ProductsSold));

            //Peoduct
            this.CreateMap<ImportProductDto, Product>();

            this.CreateMap<Product, ExportSoldProductDto>();

            this.CreateMap<Product, ExportProductDto>()
                .ForMember(d => d.BuyerName, 
                opt => opt.MapFrom(s => $"{s.Buyer.FirstName} {s.Buyer.LastName}"));

            //Category
            this.CreateMap<ImportCategoryDto, Category>();

            this.CreateMap<Category, ExportCategoryDto>()
                .ForMember(d => d.ProductsCount, 
                    opt => opt.MapFrom(s => s.CategoryProducts.Count()))
                .ForMember(d => d.AveragePrice, 
                    opt => opt.MapFrom(s => s.CategoryProducts.Average(p => p.Product.Price)))
                .ForMember(d => d.TotalRevenue, 
                    opt => opt.MapFrom(s => s.CategoryProducts.Sum(p => p.Product.Price)));

            //Category-Peoduct
            this.CreateMap<ImportCategoryProductDto, CategoryProduct>();
        }
    }
}
