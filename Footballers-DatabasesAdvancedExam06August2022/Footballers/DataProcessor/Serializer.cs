namespace Footballers.DataProcessor
{
    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using Data;
    using Footballers.DataProcessor.ExportDto;
    using Footballers.Utilities;
    using Newtonsoft.Json;
    using System.Globalization;

    public class Serializer
    {
        public static string ExportCoachesWithTheirFootballers(FootballersContext context)
        {
            XmlParser xmlParser = new XmlParser();

            IMapper mapper = AutoMapperConfiguration.CreateMapper();

            ExportCoachDto[] exportCoach = context.Coaches
                .Where(c => c.Footballers.Any())
                .OrderByDescending(c => c.Footballers.Count())
                .ThenBy(c => c.Name)
                .ProjectTo<ExportCoachDto>(mapper.ConfigurationProvider)
                .ToArray();

            return xmlParser.Serialize(exportCoach, "Coaches");
        }

        public static string ExportTeamsWithMostFootballers(FootballersContext context, DateTime date)
        {
            var teams = context.Teams
                .Where(t => t.TeamsFootballers.Any(f => f.Footballer.ContractStartDate >= date))
                .Select(t => new 
                {
                    Name = t.Name,
                    Footballers = t.TeamsFootballers
                    .Where(f => f.Footballer.ContractStartDate >= date)
                    .OrderByDescending(f => f.Footballer.ContractEndDate)
                    .ThenBy(f => f.Footballer.Name)
                    .Select(f => new
                    {
                        FootballerName = f.Footballer.Name,
                        ContractStartDate = f.Footballer.ContractStartDate.ToString("d", CultureInfo.InvariantCulture),
                        ContractEndDate = f.Footballer.ContractEndDate.ToString("d", CultureInfo.InvariantCulture),
                        BestSkillType = f.Footballer.BestSkillType.ToString(),
                        PositionType = f.Footballer.PositionType.ToString()
                    })
                    .ToList()
                })
                .OrderByDescending(t => t.Footballers.Count())
                .ThenBy(t => t.Name)
                .Take(5)
                .ToList();

            return JsonConvert.SerializeObject(teams, Formatting.Indented);
        }
    }
}
