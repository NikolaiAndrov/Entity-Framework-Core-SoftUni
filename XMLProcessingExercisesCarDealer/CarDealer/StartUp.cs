namespace CarDealer
{
    using AutoMapper;
    using CarDealer.Data;
    using CarDealer.DTOs.Import;
    using CarDealer.Models;
    using CarDealer.Utilities;
    using System.IO;

    public class StartUp
    {
        public static void Main()
        {
            CarDealerContext context = new CarDealerContext();
            string inputXml = File.ReadAllText("../../../Datasets/cars.xml");

            string result = ImportCars(context, inputXml);
            Console.WriteLine(result);
        }

        //P09 Import Suppliers
        public static string ImportSuppliers(CarDealerContext context, string inputXml)
        {
            IMapper mapper = CreateMapper();

            XmlHelper xmlHelper = new XmlHelper();
            ImportSupplierDto[] importSupplierDtos = xmlHelper.Deserialize<ImportSupplierDto[]>(inputXml, "Suppliers");

            ICollection<Supplier> suppliersToAdd = new HashSet<Supplier>();

            foreach (var dto in importSupplierDtos)
            {
                if (string.IsNullOrEmpty(dto.Name))
                {
                    continue;
                }

                Supplier supplier = mapper.Map<Supplier>(dto);
                suppliersToAdd.Add(supplier);
            }

            context.Suppliers.AddRange(suppliersToAdd);
            context.SaveChanges();

            return $"Successfully imported {suppliersToAdd.Count}";
        }

        //P10 Import Parts
        public static string ImportParts(CarDealerContext context, string inputXml)
        {
            IMapper mapper = CreateMapper();

            XmlHelper xmlHelper = new XmlHelper();
            ICollection<ImportPartDto> partDtos = xmlHelper.Deserialize<ImportPartDto[]>(inputXml, "Parts");

            ICollection<Part> partsToAdd = new HashSet<Part>();

            ICollection<int> supplierIds = context.Suppliers
                .Select(x => x.Id)
                .ToHashSet();

            foreach (var dto in partDtos)
            {
                if (string.IsNullOrEmpty(dto.Name) || 
                    dto.SupplierId == null || 
                    !supplierIds.Contains(dto.SupplierId.Value))
                {
                    continue;
                }

                Part part = mapper.Map<Part>(dto);
                partsToAdd.Add(part);
            }

            context.Parts.AddRange(partsToAdd);
            context.SaveChanges();

            return $"Successfully imported {partsToAdd.Count}";
        }

        //P11 Import Cars
        public static string ImportCars(CarDealerContext context, string inputXml)
        {
            IMapper mapper = CreateMapper();

            XmlHelper xmlHelper = new XmlHelper();

            ICollection<ImportCarDto> importCarDtos = xmlHelper.Deserialize<HashSet<ImportCarDto>>(inputXml, "Cars");

            ICollection<int> existingPartIds = context.Parts
                .Select(x => x.Id)
                .ToHashSet();

            ICollection<Car> carsToAdd = new HashSet<Car>();

            foreach (var dto in importCarDtos)
            {
                if (string.IsNullOrEmpty(dto.Make) || string.IsNullOrEmpty(dto.Model))
                {
                    continue;
                }

                Car car = mapper.Map<Car>(dto);

                foreach (var id in dto.Parts.DistinctBy(p => p.PartId))
                {
                    if (!existingPartIds.Contains(id.PartId))
                    {
                        continue;
                    }

                    car.PartsCars.Add(new PartCar { PartId = id.PartId});
                }

                carsToAdd.Add(car);
            }

            context.Cars.AddRange(carsToAdd);
            context.SaveChanges();

            return $"Successfully imported {carsToAdd.Count}";
        }

        public static IMapper CreateMapper()
        {
            MapperConfiguration mapperConfiguration = new MapperConfiguration(config =>
            {
                config.AddProfile<CarDealerProfile>();
            });

            IMapper mapper = mapperConfiguration.CreateMapper();

            return mapper;
        }
    }
}