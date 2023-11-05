namespace Theatre.DataProcessor
{
    using System.ComponentModel.DataAnnotations;
    using System.Text;
    using Theatre.Data;
    using Theatre.Data.Models.Enums;
    using Theatre.DataProcessor.ImportDto;
    using Theatre.Utilities;
    using System.Globalization;
    using Theatre.Data.Models;
    using AutoMapper;
    using Newtonsoft.Json;
    using System.Xml.Serialization;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data!";

        private const string SuccessfulImportPlay
            = "Successfully imported {0} with genre {1} and a rating of {2}!";

        private const string SuccessfulImportActor
            = "Successfully imported actor {0} as a {1} character!";

        private const string SuccessfulImportTheatre
            = "Successfully imported theatre {0} with #{1} tickets!";

        public static string ImportPlays(TheatreContext context, string xmlString)
        {
            StringBuilder sb = new StringBuilder();

            XmlParser xmlParser = new XmlParser();

            ImportPlayDto[] importPlayDtos = xmlParser.Deserialize<ImportPlayDto[]>(xmlString, "Plays");

            ICollection<Play> validPlays = new HashSet<Play>();

            foreach (var dto in importPlayDtos)
            {
                if (!Enum.TryParse(dto.Genre, true, out Genre genre))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                if (!TimeSpan.TryParseExact(dto.Duration, "c", CultureInfo.InvariantCulture, out TimeSpan duration))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                if (!IsValid(dto) || duration.TotalHours < 1)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Play play = new Play
                {
                    Title = dto.Title,
                    Duration = duration,
                    Rating = dto.Rating,
                    Genre = genre,
                    Description = dto.Description,
                    Screenwriter = dto.Screenwriter
                };

                validPlays.Add(play);
                sb.AppendLine(string.Format(SuccessfulImportPlay, play.Title, play.Genre.ToString(), play.Rating));
            }

            context.Plays.AddRange(validPlays);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportCasts(TheatreContext context, string xmlString)
        {
            StringBuilder sb = new StringBuilder();

            XmlParser xmlParser = new XmlParser();

            IMapper mapper = AutomapperConfiguration.CreateMapper();

            ImportCastDto[] importCastDtos = xmlParser.Deserialize<ImportCastDto[]>(xmlString, "Casts");

            ICollection<Cast> validCasts = new HashSet<Cast>();

            foreach (var dto in importCastDtos)
            {
                if (!IsValid(dto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Cast cast = mapper.Map<Cast>(dto);
                validCasts.Add(cast);

                sb.AppendLine(string.Format(SuccessfulImportActor, cast.FullName, cast.IsMainCharacter ? "main" : "lesser"));
                validCasts.Add(cast);
            }

            context.Casts.AddRange(validCasts);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportTtheatersTickets(TheatreContext context, string jsonString)
        {
            StringBuilder sb = new StringBuilder();

            IMapper mapper = AutomapperConfiguration.CreateMapper();

            ImportTheatreDto[] importTheatreDtos = JsonConvert.DeserializeObject<ImportTheatreDto[]>(jsonString);

            ICollection<Theatre> validTheatres = new HashSet<Theatre>();

            foreach (var dto in importTheatreDtos)
            {
                if (!IsValid(dto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Theatre theatre = mapper.Map<Theatre>(dto);

                foreach (var currentTicket in dto.Tickets)
                {
                    if (!IsValid(currentTicket))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    Ticket ticket = mapper.Map<Ticket>(currentTicket);

                    theatre.Tickets.Add(ticket);
                }

                validTheatres.Add(theatre);
                sb.AppendLine(string.Format(SuccessfulImportTheatre, theatre.Name, theatre.Tickets.Count()));
            }

            context.Theatres.AddRange(validTheatres);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        private static bool IsValid(object obj)
        {
            var validator = new ValidationContext(obj);
            var validationRes = new List<ValidationResult>();

            var result = Validator.TryValidateObject(obj, validator, validationRes, true);
            return result;
        }
    }
}
