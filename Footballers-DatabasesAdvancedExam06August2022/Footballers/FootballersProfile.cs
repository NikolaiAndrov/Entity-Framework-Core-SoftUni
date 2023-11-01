namespace Footballers
{
    using AutoMapper;
    using Footballers.Data.Models;
    using Footballers.DataProcessor.ImportDto;

    public class FootballersProfile : Profile
    {
        public FootballersProfile()
        {
            //Team
            this.CreateMap<ImportTeamDto, Team>()
                .ForMember(d => d.TeamsFootballers, opt => opt.Ignore());
        }
    }
}
