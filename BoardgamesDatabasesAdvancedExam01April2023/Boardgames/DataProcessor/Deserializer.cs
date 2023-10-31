namespace Boardgames.DataProcessor
{
    using System.ComponentModel.DataAnnotations;
    using System.Text;
    using AutoMapper;
    using Boardgames.Data;
    using Boardgames.Data.Models;
    using Boardgames.DataProcessor.ImportDto;
    using Boardgames.Utilities;
    using Newtonsoft.Json;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data!";

        private const string SuccessfullyImportedCreator
            = "Successfully imported creator – {0} {1} with {2} boardgames.";

        private const string SuccessfullyImportedSeller
            = "Successfully imported seller - {0} with {1} boardgames.";

        public static string ImportCreators(BoardgamesContext context, string xmlString)
        {
            StringBuilder sb = new StringBuilder();
            
            IMapper mapper = AutoMapperConfiguration.CreateMapper();

            XmlParser xmlParser = new XmlParser();

            ImportCreatorDto[] creatorDtos = xmlParser.Deserialize<ImportCreatorDto[]>(xmlString, "Creators");

            ICollection<Creator> validCreators = new HashSet<Creator>();

            foreach (var creatorDto in creatorDtos)
            {
                if (!IsValid(creatorDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Creator creator = mapper.Map<Creator>(creatorDto);

                foreach (var boardgameDto in creatorDto.Boardgames)
                {
                    if (!IsValid(boardgameDto))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    Boardgame boardgame = mapper.Map<Boardgame>(boardgameDto);
                    creator.Boardgames.Add(boardgame);
                }

                validCreators.Add(creator);
                sb.AppendLine(string.Format(SuccessfullyImportedCreator, creator.FirstName, creator.LastName, creator.Boardgames.Count()));
            }

            context.Creators.AddRange(validCreators);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportSellers(BoardgamesContext context, string jsonString)
        {
            StringBuilder sb = new StringBuilder();

            IMapper mapper = AutoMapperConfiguration.CreateMapper();

            ImportSellerDto[] sellerDtos = JsonConvert.DeserializeObject<ImportSellerDto[]>(jsonString);

            ICollection<int> existingBoargamesIds = context.Boardgames
                .Select(b => b.Id)
                .ToHashSet();

            ICollection<Seller> validSellers = new HashSet<Seller>();

            foreach (var sellerDto in sellerDtos)
            {
                if (!IsValid(sellerDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Seller seller = mapper.Map<Seller>(sellerDto);

                foreach (var boardgameId in sellerDto.BoardgameIds)
                {
                    if (!existingBoargamesIds.Contains(boardgameId))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    BoardgameSeller boardgameSeller = new BoardgameSeller
                    {
                        BoardgameId = boardgameId,
                        Seller = seller
                    };

                    seller.BoardgamesSellers.Add(boardgameSeller);
                }

                validSellers.Add(seller);
                sb.AppendLine(string.Format(SuccessfullyImportedSeller, seller.Name, seller.BoardgamesSellers.Count()));
            }

            context.Sellers.AddRange(validSellers);
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
