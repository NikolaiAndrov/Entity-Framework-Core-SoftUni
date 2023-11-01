namespace Footballers
{
    using AutoMapper;
    using Footballers.Data.Models;
    using Footballers.DataProcessor.ExportDto;
    using Footballers.DataProcessor.ImportDto;

    public class FootballersProfile : Profile
    {
        public FootballersProfile()
        {
            //Team
            this.CreateMap<ImportTeamDto, Team>()
                .ForMember(d => d.TeamsFootballers, opt => opt.Ignore());

            //Footballer
            this.CreateMap<Footballer, ExportFootballerDto>()
                .ForMember(d => d.Position,
                opt => opt.MapFrom(s => s.PositionType.ToString()));

            //Coach
            this.CreateMap<Coach, ExportCoachDto>()
                .ForMember(d => d.FootballersCount, 
                opt => opt.MapFrom(s => s.Footballers.Count()))
                .ForMember(d => d.Footballers, 
                opt => opt.MapFrom(s => s.Footballers.OrderBy(f => f.Name)));
        }
    }
}
