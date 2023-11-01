namespace Footballers.DataProcessor
{
    using AutoMapper;
    using Footballers.Data;
    using Footballers.Data.Models;
    using Footballers.Data.Models.Enums;
    using Footballers.DataProcessor.ImportDto;
    using Footballers.Utilities;
    using Newtonsoft.Json;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.Text;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data!";

        private const string SuccessfullyImportedCoach
            = "Successfully imported coach - {0} with {1} footballers.";

        private const string SuccessfullyImportedTeam
            = "Successfully imported team - {0} with {1} footballers.";

        public static string ImportCoaches(FootballersContext context, string xmlString)
        {
            StringBuilder sb = new StringBuilder();

            XmlParser parser = new XmlParser();

            ImportCoachDto[] importCoachDtos = parser.Deserialize<ImportCoachDto[]>(xmlString, "Coaches");

            ICollection<Coach> validCoaches = new HashSet<Coach>();

            foreach (var coachDto in importCoachDtos)
            {
                if (!IsValid(coachDto) || string.IsNullOrEmpty(coachDto.Nationality))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Coach coach = new Coach
                {
                    Name = coachDto.Name,
                    Nationality = coachDto.Nationality
                };

                foreach (var footballerDto in coachDto.Footballers)
                {
                    DateTime contractStartDate;
                    DateTime contractEndDate;
                    string dateFormat = @"dd/MM/yyyy";

                    try
                    {
                        contractStartDate = DateTime.ParseExact(footballerDto.ContractStartDate, dateFormat, CultureInfo.InvariantCulture);
                        contractEndDate = DateTime.ParseExact(footballerDto.ContractEndDate, dateFormat, CultureInfo.InvariantCulture);
                    }
                    catch (Exception)
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    if (!IsValid(footballerDto) || contractEndDate < contractStartDate)
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    Footballer footballer = new Footballer
                    {
                        Name = footballerDto.Name,
                        ContractStartDate = contractStartDate,
                        ContractEndDate = contractEndDate,
                        BestSkillType = (BestSkillType)footballerDto.BestSkillType,
                        PositionType = (PositionType)footballerDto.PositionType
                    };

                    coach.Footballers.Add(footballer);
                }

                validCoaches.Add(coach);
                sb.AppendLine(string.Format(SuccessfullyImportedCoach, coach.Name, coach.Footballers.Count()));
            }

            context.Coaches.AddRange(validCoaches);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportTeams(FootballersContext context, string jsonString)
        {
            StringBuilder sb = new StringBuilder();

            IMapper mapper = AutoMapperConfiguration.CreateMapper();

            ICollection<int> validFootollerIds = context.Footballers
                .Select(x => x.Id)
                .ToHashSet();

            ICollection<Team> validTeams = new HashSet<Team>();

            ImportTeamDto[] impportTeamDtos = JsonConvert.DeserializeObject<ImportTeamDto[]>(jsonString);

            foreach (var teamDto in impportTeamDtos)
            {
                if (!IsValid(teamDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Team team = mapper.Map<Team>(teamDto);

                foreach (var footollerId in teamDto.FootballerIds)
                {
                    if (!validFootollerIds.Contains(footollerId))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    TeamFootballer teamFootballer = new TeamFootballer
                    {
                        Team = team,
                        FootballerId = footollerId
                    };

                    team.TeamsFootballers.Add(teamFootballer);
                }

                validTeams.Add(team);
                sb.AppendLine(string.Format(SuccessfullyImportedTeam, team.Name, team.TeamsFootballers.Count()));
            }

            context.Teams.AddRange(validTeams);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        private static bool IsValid(object dto)
        {
            var validationContext = new ValidationContext(dto);
            var validationResult = new List<ValidationResult>();

            return Validator.TryValidateObject(dto, validationContext, validationResult, true);
        }
    }
}
