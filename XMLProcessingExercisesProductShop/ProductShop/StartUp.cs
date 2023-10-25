namespace ProductShop
{
    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using ProductShop.Data;
    using ProductShop.DTOs.Export;
    using ProductShop.DTOs.Import;
    using ProductShop.Models;
    using ProductShop.Utilities;

    public class StartUp
    {
        public static void Main()
        {
            ProductShopContext context = new ProductShopContext();
            //string inputXml = File.ReadAllText(@"../../../Datasets/categories-products.xml");

            string result = GetUsersWithProducts(context);
            Console.WriteLine(result);
        }

        //P01 Import Users
        public static string ImportUsers(ProductShopContext context, string inputXml)
        {
            XmlParser xmlParser = new XmlParser();
            IMapper mapper = CreateMapper();

            ImportUserDto[] importUserDtos = xmlParser.Deserialize<ImportUserDto[]>(inputXml, "Users");
            User[] users = mapper.Map<User[]>(importUserDtos);

            context.Users.AddRange(users);
            context.SaveChanges();

            return $"Successfully imported {users.Length}";
        }

        //P02 Import Products
        public static string ImportProducts(ProductShopContext context, string inputXml)
        {
            XmlParser xmlParser = new XmlParser();
            IMapper mapper = CreateMapper();

            ImportProductDto[] importProductDtos = xmlParser.Deserialize<ImportProductDto[]>(inputXml, "Products");
            ICollection<Product> products = mapper.Map<Product[]>(importProductDtos);

            context.Products.AddRange(products);
            context.SaveChanges();

            return $"Successfully imported {products.Count}";
        }

        //P03 Import Categories
        public static string ImportCategories(ProductShopContext context, string inputXml)
        {
            XmlParser xmlParser = new XmlParser();
            IMapper mapper = CreateMapper();

            ImportCategoryDto[] importCategoryDtos = xmlParser.Deserialize<ImportCategoryDto[]>(inputXml, "Categories");
            ICollection<Category> categories = new HashSet<Category>();

            foreach (var dto in importCategoryDtos)
            {
                if (dto.Name == null)
                {
                    continue;
                }

                Category category = mapper.Map<Category>(dto);  
                categories.Add(category);
            }

            context.Categories.AddRange(categories);
            context.SaveChanges();

            return $"Successfully imported {categories.Count}";
        }

        //P04Import Categories and Products
        public static string ImportCategoryProducts(ProductShopContext context, string inputXml)
        {
            XmlParser xmlParser = new XmlParser();
            IMapper mapper = CreateMapper();

            ICollection<int> allProducts = context.Products
                .Select(p => p.Id)
                .ToHashSet();

            ICollection<int> allCategories = context.Categories
                .Select(c => c.Id)
                .ToHashSet();

            ImportCategoryProductDto[] categoryProductDtos = xmlParser.Deserialize<ImportCategoryProductDto[]>(inputXml, "CategoryProducts");

            ICollection<CategoryProduct> categoryProducts = new HashSet<CategoryProduct>();

            foreach (var dto in categoryProductDtos)
            {
                if (!allProducts.Contains(dto.ProductId) || 
                    !allCategories.Contains(dto.CategoryId))
                {
                    continue;
                }

                CategoryProduct categoryProduct = mapper.Map<CategoryProduct>(dto);
                categoryProducts.Add(categoryProduct);
            }

            context.CategoryProducts.AddRange(categoryProducts);
            context.SaveChanges();

            return $"Successfully imported {categoryProducts.Count}";
        }

        //P05 Export Products In Range
        public static string GetProductsInRange(ProductShopContext context)
        {
            XmlParser xmlParser = new XmlParser();
            IMapper mapper = CreateMapper();

            var products = context.Products
                .Where(p => p.Price >= 500 && p.Price <= 1000)
                .OrderBy(p => p.Price)
                .Take(10)
                .ProjectTo<ExportProductDto>(mapper.ConfigurationProvider)
                .ToArray();

            return xmlParser.Serialize(products, "Products");
        }

        //P06 Export Sold Products
        public static string GetSoldProducts(ProductShopContext context)
        {
            XmlParser xmlParser = new XmlParser();
            IMapper mapper = CreateMapper();

            var usersWithProducts = context.Users
                .Where(u => u.ProductsSold.Count() > 0)
                .OrderBy(u => u.LastName)
                .ThenBy(u => u.FirstName)
                .Take(5)
                .ProjectTo<ExportUserWithSoldProductDto>(mapper.ConfigurationProvider)
                .ToArray();



            return xmlParser.Serialize<ExportUserWithSoldProductDto[]>(usersWithProducts, "Users");
        }

        //P07 Export Categories By Products Count
        public static string GetCategoriesByProductsCount(ProductShopContext context)
        {
            XmlParser xmlParser = new XmlParser();
            IMapper mapper = CreateMapper();

            var categories = context.Categories
                .ProjectTo<ExportCategoryDto>(mapper.ConfigurationProvider)
                .OrderByDescending(p => p.ProductsCount)
                .ThenBy(p => p.TotalRevenue)
                .ToArray();

            return xmlParser.Serialize(categories, "Categories");
        }

        //P08 Export Users and Products
        public static string GetUsersWithProducts(ProductShopContext context)
        {
            XmlParser xmlParser = new XmlParser();

            var users = context.Users
                .Where(u => u.ProductsSold.Count() > 0)
                .OrderByDescending(u => u.ProductsSold.Count())
                .Take(10)
                .Select(x => new ExportWrappedUserDto
                {
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    Age = x.Age,
                    SoldProductsCount = new ExportSoldProductsCountDto
                    {
                        SoldProductsCount = x.ProductsSold.Count(),
                        Products = x.ProductsSold.Select(l => new ExporWrappedSoldProductDto
                        {
                            Name = l.Name,
                            Price = l.Price
                        })
                        .OrderByDescending(p => p.Price)
                        .ToArray()
                    }
                })
                .ToArray();
             
            ExportFinalWrapperUserDto export = new ExportFinalWrapperUserDto
            {
                TotalUsersCount = context.Users.Where(u => u.ProductsSold.Count > 0).Count(),
                Users = users
            };
    

            return xmlParser.Serialize(export, "Users");
        }

        public static IMapper CreateMapper()
        {
            MapperConfiguration mapperConfiguration = new MapperConfiguration(configuration =>
            {
                configuration.AddProfile<ProductShopProfile>();
            });

            IMapper mapper = mapperConfiguration.CreateMapper();

            return mapper;
        }
    }
}