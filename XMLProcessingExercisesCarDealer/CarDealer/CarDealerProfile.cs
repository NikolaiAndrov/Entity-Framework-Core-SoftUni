namespace CarDealer
{
    using AutoMapper;
    using CarDealer.DTOs.Export;
    using CarDealer.DTOs.Import;
    using CarDealer.Models;

    public class CarDealerProfile : Profile
    {
        public CarDealerProfile()
        {
            //Suplier
            this.CreateMap<ImportSupplierDto, Supplier>();

            this.CreateMap<Supplier, ExportLocalSuppliersDto>()
                .ForMember(d => d.PartsCount,
                opt => opt.MapFrom(s => s.Parts.Count()));

            //Part
            this.CreateMap<ImportPartDto, Part>()
                .ForMember(d => d.SupplierId, 
                opt => opt.MapFrom(s => s.SupplierId!.Value));

            this.CreateMap<Part, ExportCarPartDto>();

            //Car
            this.CreateMap<ImportCarDto, Car>()
                .ForSourceMember(s => s.Parts, 
                opt => opt.DoNotValidate());

            this.CreateMap<Car, ExportCarDto>();

            this.CreateMap<Car, ExportBmwCarDto>();

            this.CreateMap<Car, ExportCartWithListOfPartsDto>()
                .ForMember(d => d.CarParts, 
                opt => opt.MapFrom(s => s.PartsCars
                .Select(cp => cp.Part)
                .OrderByDescending(p => p.Price)
                .ToArray()));

            //Customer
            this.CreateMap<ImportCustomerDto, Customer>()
                .ForMember(d => d.BirthDate, 
                opt => opt.MapFrom(s => DateTime.Parse(s.BirthDate)));

            this.CreateMap<ExportCustomerDto, ExportCustomerOutputDto>()
                .ForMember(d => d.SpentMoney, 
                opt => opt.MapFrom(s => s.SpentMoney.ToString("f2")));

            //Sale
            this.CreateMap<ImportSaleDto, Sale>()
                .ForMember(d => d.CarId,
                opt => opt.MapFrom(s => s.CarId!.Value));
        }
    }
}
