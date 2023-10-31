namespace Boardgames.DataProcessor.ImportDto
{
    using Boardgames.Common;
    using Newtonsoft.Json;
    using System.ComponentModel.DataAnnotations;

    public class ImportSellerDto
    {
        public ImportSellerDto()
        {
            BoardgameIds = new HashSet<int>();
        }

        [Required]
        [MinLength(ValidationConstants.SellerNameMinLength)]
        [MaxLength(ValidationConstants.SellerNameMaxLength)]
        [JsonProperty("Name")]
        public string Name { get; set; } = null!;

        [Required]
        [MinLength(ValidationConstants.SellerAddressMinLength)]
        [MaxLength(ValidationConstants.SellerAddressMaxLength)]
        [JsonProperty("Address")]
        public string Address { get; set; } = null!;

        [Required]
        [JsonProperty("Country")]
        public string Country { get; set; } = null!;

        [Required]
        [RegularExpression(ValidationConstants.SellerWebsiteRegex)]
        [JsonProperty("Website")]
        public string Website { get; set; } = null!;

        [JsonProperty("Boardgames")]
        public ICollection<int> BoardgameIds { get; set; }
    }
}
