﻿namespace CarDealer
{
    using AutoMapper;
    using CarDealer.Data;
    using CarDealer.DTOs.Import;
    using CarDealer.Models;
    using CarDealer.Utilities;
    using Castle.Core.Resource;
    using System.IO;

    public class StartUp
    {
        public static void Main()
        {
            CarDealerContext context = new CarDealerContext();
            string inputXml = File.ReadAllText("../../../Datasets/sales.xml");

            string result = ImportSales(context, inputXml);
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

        //P12 Import Customers
        public static string ImportCustomers(CarDealerContext context, string inputXml)
        {
            IMapper mapper = CreateMapper();
            XmlHelper xmlHelper = new XmlHelper();

            ICollection<ImportCustomerDto> importCustomerDtos = xmlHelper.Deserialize<HashSet<ImportCustomerDto>>(inputXml, "Customers");

            ICollection<Customer> customersToAdd = new HashSet<Customer>();

            foreach (var dto in importCustomerDtos)
            {
                if (string.IsNullOrEmpty(dto.Name) || string.IsNullOrEmpty(dto.BirthDate))
                {
                    continue;
                }

                Customer customer = mapper.Map<Customer>(dto);
                customersToAdd.Add(customer);
            }

            context.Customers.AddRange(customersToAdd);
            context.SaveChanges();

            return $"Successfully imported {customersToAdd.Count}";
        }

        //P13 Import Sales
        public static string ImportSales(CarDealerContext context, string inputXml)
        {
            IMapper mapper = CreateMapper();
            XmlHelper xmlHelper = new XmlHelper();
            ICollection<int> carIds = context.Cars
                .Select(x => x.Id)
                .ToHashSet();

            ICollection<ImportSaleDto> importSaleDtos = xmlHelper.Deserialize<HashSet<ImportSaleDto>>(inputXml, "Sales");

            ICollection<Sale> salesToAdd = new HashSet<Sale>();

            foreach (var dto in importSaleDtos)
            {
                if (!dto.CarId.HasValue || !carIds.Contains(dto.CarId.Value))
                {
                    continue;
                }

                Sale sale = mapper.Map<Sale>(dto);
                salesToAdd.Add(sale);
            }

            context.Sales.AddRange(salesToAdd);
            context.SaveChanges();

            return $"Successfully imported {salesToAdd.Count}";
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