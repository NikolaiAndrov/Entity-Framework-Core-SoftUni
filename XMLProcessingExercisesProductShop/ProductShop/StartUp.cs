namespace ProductShop
{
    using AutoMapper;
    using ProductShop.Data;
    using ProductShop.DTOs.Import;
    using ProductShop.Models;
    using ProductShop.Utilities;

    public class StartUp
    {
        public static void Main()
        {
            ProductShopContext context = new ProductShopContext();
            string inputXml = File.ReadAllText(@"../../../Datasets/categories.xml");

            string result = ImportCategories(context, inputXml);
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