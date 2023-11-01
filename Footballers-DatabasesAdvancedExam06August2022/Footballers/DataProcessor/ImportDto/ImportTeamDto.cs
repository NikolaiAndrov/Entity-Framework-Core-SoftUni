namespace Footballers.DataProcessor.ImportDto
{
    using Footballers.Common;
    using Newtonsoft.Json;
    using System.ComponentModel.DataAnnotations;

    public class ImportTeamDto
    {
        public ImportTeamDto()
        {
            this.FootballerIds = new HashSet<int>();
        }

        [Required]
        [MinLength(ValidationConstants.TeamNameMinLength)]
        [MaxLength(ValidationConstants.TeamNameMaxLength)]
        [RegularExpression(ValidationConstants.TeamNameRegex)]
        [JsonProperty("Name")]
        public string Name { get; set; } = null!;

        [Required]
        [MinLength(ValidationConstants.TeamNationalityMinLength)]
        [MaxLength(ValidationConstants.TeamNationalityMaxLength)]
        [JsonProperty("Nationality")]
        public string Nationality { get; set; } = null!;

        [Required]
        [Range(ValidationConstants.TeamTrophiesMinValue, ValidationConstants.TeamTrophiesMaxValue)]
        [JsonProperty("Trophies")]
        public int Trophies { get; set; }

        [JsonProperty("Footballers")]
        public ICollection<int> FootballerIds { get; set; }
    }
}
