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

            //Car
            this.CreateMap<ImportCarDto, Car>()
                .ForSourceMember(s => s.Parts, 
                opt => opt.DoNotValidate());

            this.CreateMap<Car, ExportCarDto>();

            this.CreateMap<Car, ExportBmwCarDto>();

            //Customer
            this.CreateMap<ImportCustomerDto, Customer>()
                .ForMember(d => d.BirthDate, 
                opt => opt.MapFrom(s => DateTime.Parse(s.BirthDate)));

            //Sale
            this.CreateMap<ImportSaleDto, Sale>()
                .ForMember(d => d.CarId,
                opt => opt.MapFrom(s => s.CarId!.Value));

            
        }
    }
}
