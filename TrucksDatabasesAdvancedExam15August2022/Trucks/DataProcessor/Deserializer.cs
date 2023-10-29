namespace Trucks.DataProcessor
{
    using System.ComponentModel.DataAnnotations;
    using System.Text;
    using AutoMapper;
    using Data;
    using DataProcessor.ImportDto;
    using Trucks.Data.Models;
    using Utilities;
    using Newtonsoft.Json;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data!";

        private const string SuccessfullyImportedDespatcher
            = "Successfully imported despatcher - {0} with {1} trucks.";

        private const string SuccessfullyImportedClient
            = "Successfully imported client - {0} with {1} trucks.";

        public static string ImportDespatcher(TrucksContext context, string xmlString)
        {
            StringBuilder sb = new StringBuilder();

            IMapper mapper = CreateMapper();

            XmlParser xmlParser = new XmlParser();

            ImportDespatcherDto[] importDespatcherDtos = xmlParser.Deserialize<ImportDespatcherDto[]>(xmlString, "Despatchers");

            ICollection<Despatcher> despatchers = new HashSet<Despatcher>();


            foreach (var despatcherDto in importDespatcherDtos)
            {
                if (!IsValid(despatcherDto) || string.IsNullOrEmpty(despatcherDto.Position))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Despatcher despatcher = new Despatcher
                {
                    Name = despatcherDto.Name,
                    Position = despatcherDto.Position
                };

                foreach (var truckDto in despatcherDto.Trucks)
                {
                    if (!IsValid(truckDto))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    Truck truck = mapper.Map<Truck>(truckDto);
                    despatcher.Trucks.Add(truck);
                }

                despatchers.Add(despatcher);
                sb.AppendLine(string.Format(SuccessfullyImportedDespatcher, despatcher.Name, despatcher.Trucks.Count()));
            }

            context.Despatchers.AddRange(despatchers);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }
        public static string ImportClient(TrucksContext context, string jsonString)
        {
            StringBuilder sb = new StringBuilder();

            IMapper mapper = CreateMapper();

            ImportClientDto[] importClientDtos = JsonConvert.DeserializeObject<ImportClientDto[]>(jsonString);

            ICollection<int> existingTrucks = context.Trucks
                .Select(x => x.Id)
                .ToHashSet();

            ICollection<Client> clients = new HashSet<Client>();

            foreach (var clientDto in importClientDtos)
            {
                if (!IsValid(clientDto) || clientDto.Type == "usual")
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Client client = mapper.Map<Client>(clientDto);

                foreach (var truckId in clientDto.TruckIds)
                {
                    if (!existingTrucks.Contains(truckId))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    ClientTruck clientTruck = new ClientTruck
                    {
                        Client = client,
                        TruckId = truckId
                    };

                    client.ClientsTrucks.Add(clientTruck);
                }

                clients.Add(client);
                sb.AppendLine(string.Format(SuccessfullyImportedClient, client.Name, client.ClientsTrucks.Count()));
            }

            context.Clients.AddRange(clients);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        private static bool IsValid(object dto)
        {
            var validationContext = new ValidationContext(dto);
            var validationResult = new List<ValidationResult>();

            return Validator.TryValidateObject(dto, validationContext, validationResult, true);
        }

        private static IMapper CreateMapper()
        {
            MapperConfiguration configuration = new MapperConfiguration(opt =>
            {
                opt.AddProfile<TrucksProfile>();
            });

            IMapper mapper = configuration.CreateMapper();

            return mapper;
        }
    }
}