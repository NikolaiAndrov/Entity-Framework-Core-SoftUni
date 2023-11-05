namespace Theatre.DataProcessor.ImportDto
{
    using Newtonsoft.Json;
    using System.ComponentModel.DataAnnotations;
    using Theatre.Common;

    public class ImportTheatreDto
    {
        public ImportTheatreDto()
        {
            this.Tickets = new HashSet<ImportTicketDto>();
        }

        [Required]
        [MinLength(ValidationConstants.TheatreNameMinLength)]
        [MaxLength(ValidationConstants.TheatreNameMaxLength)]
        [JsonProperty("Name")]
        public string Name { get; set; } = null!;

        [Required]
        [Range(ValidationConstants.TheatreNumberOfHallsMinValue, ValidationConstants.TheatreNumberOfHallsMaxValue)]
        [JsonProperty("NumberOfHalls")]
        public sbyte NumberOfHalls { get; set; }

        [Required]
        [MinLength(ValidationConstants.TheatreDirectorMinLength)]
        [MaxLength(ValidationConstants.TheatreDirectorMaxLength)]
        [JsonProperty("Director")]
        public string Director { get; set; } = null!;

        [JsonProperty("Tickets")]
        public ICollection<ImportTicketDto> Tickets { get; set; }
    }
}
