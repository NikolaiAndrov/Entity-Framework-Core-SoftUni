namespace ProductShop
{
    using ProductShop.Data;
    using ProductShop.DTOs.Import;
    using Newtonsoft.Json;
    using AutoMapper;
    using ProductShop.Models;
    using Microsoft.EntityFrameworkCore;
    using AutoMapper.QueryableExtensions;
    using ProductShop.DTOs.Export;
    using Newtonsoft.Json.Serialization;
    using System.Diagnostics;
    using System.Xml.Linq;

    public class StartUp
    {
        public static void Main()
        {
            ProductShopContext context = new ProductShopContext();

            //string inputJson = File.ReadAllText(@"../../../Datasets/categories-products.json");

            string result = GetUsersWithProducts(context);
            Console.WriteLine(result);
        }

        //P01 Import Data
        public static string ImportUsers(ProductShopContext context, string inputJson)
        {
            IMapper mapper = CreateMapper();

            ImportUserDto[] usersDto = JsonConvert.DeserializeObject<ImportUserDto[]>(inputJson);

            User[] users = mapper.Map<User[]>(usersDto);

            context.Users.AddRange(users);

            context.SaveChanges();

            return $"Successfully imported {users.Length}"; ;
        }

        //02 Import Products
        public static string ImportProducts(ProductShopContext context, string inputJson)
        {
            IMapper mapper = CreateMapper();

            ImportProductDto[] productDtos = JsonConvert.DeserializeObject<ImportProductDto[]>(inputJson);

            Product[] products = mapper.Map<Product[]>(productDtos);

            context.Products.AddRange(products);

            context.SaveChanges();

            return $"Successfully imported {products.Length}";
        }

        //P03 Import Categories
        public static string ImportCategories(ProductShopContext context, string inputJson)
        {
            IMapper mapper = CreateMapper();

            ImportCategoryDto[] categoryDtos = JsonConvert.DeserializeObject<ImportCategoryDto[]>(inputJson);

            ICollection<Category> categories = new HashSet<Category>();

            foreach (var item in categoryDtos)
            {
                if (item.Name == null)
                {
                    continue;
                }

                Category category = mapper.Map<Category>(item);
                categories.Add(category);
            }

            context.Categories.AddRange(categories);
            context.SaveChanges();

            return $"Successfully imported {categories.Count}";
        }

        //P04 Import Categories and Products
        public static string ImportCategoryProducts(ProductShopContext context, string inputJson)
        {
            IMapper mapper = CreateMapper();

            ImportCategoryProductDto[] categoryProductDtos = JsonConvert.DeserializeObject<ImportCategoryProductDto[]>(inputJson);

            CategoryProduct[] categoryProducts = mapper.Map<CategoryProduct[]>(categoryProductDtos);

            context.CategoriesProducts.AddRange(categoryProducts);

            context.SaveChanges();

            return $"Successfully imported {categoryProducts.Length}";
        }

        //P05 Export Products in Range
        public static string GetProductsInRange(ProductShopContext context)
        {
            IMapper mapper = CreateMapper();

            ExportProductInRange[] products = context.Products
                .Where(p => p.Price >= 500 && p.Price <= 1000)
                .OrderBy(p => p.Price)
                .AsNoTracking()
                .ProjectTo<ExportProductInRange>(mapper.ConfigurationProvider)
                .ToArray();

            return JsonConvert.SerializeObject(products, Formatting.Indented);
        }

        //P06 Export Sold Products
        public static string GetSoldProducts(ProductShopContext context)
        {
            var users = context.Users
                .Where(u => u.ProductsSold.Any(p => p.BuyerId != null))
                .OrderBy(u => u.LastName)
                .ThenBy(u => u.FirstName)
                .AsNoTracking()
                .Select(u => new
                {
                    firstName = u.FirstName,
                    lastName = u.LastName,
                    soldProducts = u.ProductsSold
                        .Select(p => new
                        {
                            name = p.Name,
                            price = p.Price,
                            buyerFirstName = p.Buyer.FirstName,
                            buyerLastName = p.Buyer.LastName
                        })
                        .ToArray()
                })
                .ToArray();

            return JsonConvert.SerializeObject(users, Formatting.Indented);
        }

        //P07 Export Categories by Products Count
        public static string GetCategoriesByProductsCount(ProductShopContext context)
        {
            var categories = context.Categories
                .OrderByDescending(c => c.CategoriesProducts.Count())
                .Select(c => new
                {
                    category = c.Name,
                    productsCount = c.CategoriesProducts.Count(),
                    averagePrice = c.CategoriesProducts.Average(p => p.Product.Price).ToString("f2"),
                    totalRevenue = c.CategoriesProducts.Sum(p => p.Product.Price).ToString("f2")
                })
                .AsNoTracking()
                .ToArray();

            return JsonConvert.SerializeObject(categories, Formatting.Indented);
        }

        //P08 Export Users and Products
        public static string GetUsersWithProducts(ProductShopContext context)
        {
            var users = context.Users
                .Where(u => u.ProductsSold.Any(p => p.BuyerId != null))
                .Select(u => new
                {
                    firstName = u.FirstName,
                    lastName = u.LastName,
                    age = u.Age,
                    soldProducts = new
                    {
                        count = u.ProductsSold
                            .Count(p => p.BuyerId != null),
                        products = u.ProductsSold
                        .Where(p => p.BuyerId != null)
                        .Select(p => new
                        {
                            name = p.Name,
                            price = p.Price
                        })
                        .ToArray()
                    }
                })
                .AsNoTracking()
                .OrderByDescending(x => x.soldProducts.count)
                .ToArray();

            var resultUsers = new
            {
                usersCount = users.Length,
                users = users
            };



            return JsonConvert.SerializeObject(resultUsers, Formatting.Indented, new JsonSerializerSettings()
            {
                NullValueHandling = NullValueHandling.Ignore,
            });
        }

        private static IMapper CreateMapper()
        {
            MapperConfiguration config = new MapperConfiguration(config =>
            {
                config.AddProfile<ProductShopProfile>();
            });

            IMapper mapper = config.CreateMapper();

            return mapper;
        }
    }
}