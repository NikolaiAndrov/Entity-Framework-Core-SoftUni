namespace Artillery
{
    using Artillery.Data.Models;
    using Artillery.DataProcessor.ExportDto;
    using AutoMapper;

    class ArtilleryProfile : Profile
    {
        public ArtilleryProfile()
        {
            this.CreateMap<ImportCountryXmlDto, Country>();
        }
    }
}