namespace Trucks.DataProcessor.ImportDto
{
    using Newtonsoft.Json;
    using System.ComponentModel.DataAnnotations;
    using Trucks.Common;

    public class ImportClientDto
    {
        public ImportClientDto()
        {
            this.TruckIds = new HashSet<int>();
        }

        public int Id { get; set; }

        [Required]
        [MinLength(ValidationConstants.ClientNameMinLength)]
        [MaxLength(ValidationConstants.ClientNameMaxLength)]
        [JsonProperty("Name")]
        public string Name { get; set; } = null!;

        [Required]
        [MinLength(ValidationConstants.ClientNationalityMinLength)]
        [MaxLength(ValidationConstants.ClientNationalityMaxLength)]
        [JsonProperty("Nationality")]
        public string Nationality { get; set; } = null!;

        [Required]
        [JsonProperty("Type")]
        public string Type { get; set; } = null!;

        [JsonProperty("Trucks")]
        public HashSet<int> TruckIds { get; set; }
    }
}
