namespace Artillery
{
    using Artillery.Data.Models;
    using Artillery.DataProcessor.ImportDto;
    using AutoMapper;

    class ArtilleryProfile : Profile
    {
        public ArtilleryProfile()
        {
            //Country
            this.CreateMap<ImportCountryXmlDto, Country>();

            //Manufacturer
            this.CreateMap<ImportManufacturerXmlDto, Manufacturer>();

            //Shell
            this.CreateMap<ImportShellXmlDto, Shell>();
        }
    }
}