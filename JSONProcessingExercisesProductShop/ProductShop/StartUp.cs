namespace ProductShop
{
    using ProductShop.Data;
    using ProductShop.DTOs.Import;
    using Newtonsoft.Json;
    using AutoMapper;
    using ProductShop.Models;

    public class StartUp
    {
        public static void Main()
        {
            ProductShopContext context = new ProductShopContext();

            string inputJson = File.ReadAllText(@"../../../Datasets/categories-products.json");

            string result = ImportCategoryProducts(context, inputJson);
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