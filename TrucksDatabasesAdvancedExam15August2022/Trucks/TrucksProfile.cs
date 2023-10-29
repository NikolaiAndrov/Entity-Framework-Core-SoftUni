namespace Trucks
{
    using AutoMapper;
    using Trucks.Data.Models;
    using Trucks.Data.Models.Enums;
    using Trucks.DataProcessor.ImportDto;

    public class TrucksProfile : Profile
    {
        public TrucksProfile()
        {
            //Truck
            this.CreateMap<ImportTruckDto, Truck>()
                .ForMember(d => d.CategoryType,
                    //opt => opt.MapFrom(s => Enum.Parse<CategoryType>(s.CategoryType.ToString())))
                    opt => opt.MapFrom(s => (CategoryType)s.CategoryType))
                .ForMember(d => d.MakeType, 
                    //opt => opt.MapFrom(s => Enum.Parse<MakeType>(s.MakeType.ToString())))
                    opt => opt.MapFrom(s => (MakeType)s.MakeType));

            //Client
            this.CreateMap<ImportClientDto, Client>();
        }
    }
}
