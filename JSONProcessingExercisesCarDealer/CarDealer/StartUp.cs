namespace CarDealer
{
    using AutoMapper;
    using Data;
    using DTOs.Import;
    using Microsoft.EntityFrameworkCore;
    using Models;
    using Newtonsoft.Json;
    using System.IO;

    public class StartUp
    {
        public static void Main()
        {
            CarDealerContext context = new CarDealerContext();

            //string inputJson = File.ReadAllText(@"../../../Datasets/sales.json");

            string result = GetTotalSalesByCustomer(context);
            Console.WriteLine(result);
        }

        //P09 Import Suppliers
        public static string ImportSuppliers(CarDealerContext context, string inputJson)
        {
            IMapper mapper = CreateMapper();

            ImportSupplierDto[] importSupplierDtos = JsonConvert.DeserializeObject<ImportSupplierDto[]>(inputJson);

            Supplier[] suppliers = mapper.Map<Supplier[]>(importSupplierDtos);

            context.Suppliers.AddRange(suppliers);

            context.SaveChanges();

            return $"Successfully imported {suppliers.Length}.";
        }

        //P10 Import Parts
        public static string ImportParts(CarDealerContext context, string inputJson)
        {
            IMapper mapper = CreateMapper();

            ImportPartDto[] importPartDtos = JsonConvert.DeserializeObject<ImportPartDto[]>(inputJson);

            ICollection<Part> partsToAdd = new HashSet<Part>();

            foreach (var dto in importPartDtos)
            {
                if (!context.Suppliers.Any(s => s.Id == dto.SupplierId))
                {
                    continue;
                }

                Part part = mapper.Map<Part>(dto);
                partsToAdd.Add(part);
            }

            context.Parts.AddRange(partsToAdd);
            context.SaveChanges();

            return $"Successfully imported {partsToAdd.Count}.";
        }

        //P11 Import Cars
        public static string ImportCars(CarDealerContext context, string inputJson)
        {
            IMapper mapper = CreateMapper();
            ImportCarDto[] importCarDtos = JsonConvert.DeserializeObject<ImportCarDto[]>(inputJson);
            ICollection<Car> carsToAdd = new HashSet<Car>();

            foreach (var dto in importCarDtos)
            {
                Car currentCar = mapper.Map<Car>(dto);

                foreach (var id in dto.PartsId)
                {
                    if (!context.Parts.Any(p => p.Id == id))
                    {
                        continue;
                    }

                    currentCar.PartsCars.Add(new PartCar
                    {
                        PartId = id,
                    });
                }

                carsToAdd.Add(currentCar);
            }

            context.Cars.AddRange(carsToAdd);
            context.SaveChanges();

            return $"Successfully imported {carsToAdd.Count}.";
        }

        //P12 Import Customers
        public static string ImportCustomers(CarDealerContext context, string inputJson)
        {
            IMapper mapper = CreateMapper();

            ImportCustomerDto[] importCustomerDtos = JsonConvert.DeserializeObject<ImportCustomerDto[]>(inputJson);

            Customer[] customers = mapper.Map<Customer[]>(importCustomerDtos);

            context.Customers.AddRange(customers);
            context.SaveChanges();

            return $"Successfully imported {customers.Length}.";
        }

        //P13 Import Sales
        public static string ImportSales(CarDealerContext context, string inputJson)
        {
            IMapper mapper = CreateMapper();

            ImportSaleDto[] importSaleDtos = JsonConvert.DeserializeObject<ImportSaleDto[]>(inputJson);

            Sale[] salesToAdd = mapper.Map<Sale[]>(importSaleDtos);
         
            context.Sales.AddRange(salesToAdd);
            context.SaveChanges();
            return $"Successfully imported {salesToAdd.Count()}.";
        }

        //P14 Export Ordered Customers
        public static string GetOrderedCustomers(CarDealerContext context)
        {
            var customers = context.Customers
                .OrderBy(c => c.BirthDate)
                .ThenBy(c => c.IsYoungDriver)
                .AsNoTracking()
                .Select(c => new
                {
                    c.Name,
                    BirthDate = c.BirthDate.ToString("dd/MM/yyyy"),
                    c.IsYoungDriver
                })
                .ToArray();

            return JsonConvert.SerializeObject(customers, Formatting.Indented);   
        }

        //P15  Export Cars from Make Toyota
        public static string GetCarsFromMakeToyota(CarDealerContext context)
        {
            var cars = context.Cars
                .Where(c => c.Make == "Toyota")
                .OrderBy(c => c.Model)
                .ThenByDescending(c => c.TraveledDistance)
                .AsNoTracking()
                .Select(c => new
                {
                    c.Id,
                    c.Make,
                    c.Model,
                    c.TraveledDistance
                })
                .ToArray();

            return JsonConvert.SerializeObject(cars, Formatting.Indented);
        }

        //P16 Export Local Suppliers
        public static string GetLocalSuppliers(CarDealerContext context)
        {
            var suppliers = context.Suppliers
                .Where(s => s.IsImporter == false)
                .AsNoTracking()
                .Select(s => new
                {
                    s.Id,
                    s.Name,
                    PartsCount = s.Parts.Count()
                })
                .ToArray();

            return JsonConvert.SerializeObject(suppliers, Formatting.Indented);
        }

        //P17 Export Cars with Their List of Parts
        public static string GetCarsWithTheirListOfParts(CarDealerContext context)
        {
            var carsWithParts = context.Cars
                .AsNoTracking ()
                .Select(c => new
                {
                    car = new
                    {
                        c.Make,
                        c.Model,
                        c.TraveledDistance
                    },
                    parts = c.PartsCars
                        .Select(p => new
                        {
                            Name =p.Part.Name,
                            Price = p.Part.Price.ToString("f2"),
                        })
                        .ToArray()
                })
                .ToArray();

            return JsonConvert.SerializeObject(carsWithParts, Formatting.Indented);
        }

        //P18 Export Total Sales by Customer
        public static string GetTotalSalesByCustomer(CarDealerContext context)
        {
            var customers = context.Customers
                .Where(c => c.Sales.Count() > 0)
                //.ToList()
                .Select(c => new
                {
                    fullName = c.Name,
                    boughtCars = c.Sales.Count(),
                    spentMoney = c.Sales.Sum(s => s.Car.PartsCars.Sum(p => p.Part.Price)) 
                })
                .OrderByDescending(x => x.spentMoney)
                .ThenByDescending(x => x.boughtCars)
                .ToList();

            return JsonConvert.SerializeObject(customers, Formatting.Indented);
        }

        //P19 Export Sales with Applied Discount
        public static string GetSalesWithAppliedDiscount(CarDealerContext context)
        {
            var sales = context.Sales
                .Take(10)
                .ToList()
                .Select(s => new
                {
                    car = new
                    {
                        s.Car.Make,
                        s.Car.Model,
                        s.Car.TraveledDistance
                    },

                    customerName = s.Customer.Name,
                    discount = s.Discount.ToString("f2"),
                    price = (s.Car.PartsCars.Sum(x => x.Part.Price)).ToString("f2"),
                    priceWithDiscount = ((s.Car.PartsCars.Sum(x => x.Part.Price)) * (1 - (s.Discount / 100))).ToString("f2")

                })
                .ToList();

            return JsonConvert.SerializeObject(sales, Formatting.Indented);
        }

        public static IMapper CreateMapper()
        {
            MapperConfiguration configuration = new MapperConfiguration(config =>
            {
                config.AddProfile<CarDealerProfile>();
            });

            IMapper mapper = configuration.CreateMapper();

            return mapper;
        }
    }
}