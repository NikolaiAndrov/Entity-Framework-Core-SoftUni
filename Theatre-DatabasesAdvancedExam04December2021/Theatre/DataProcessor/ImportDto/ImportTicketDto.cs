namespace Theatre.DataProcessor.ImportDto
{
    using Newtonsoft.Json;
    using System.ComponentModel.DataAnnotations;
    using Theatre.Common;

    public class ImportTicketDto
    {
        [Required]
        [Range(typeof(decimal), ValidationConstants.TicketPriceMinValue, ValidationConstants.TicketPriceMaxValue)]
        [JsonProperty("Price")]
        public decimal Price { get; set; }

        [Required]
        [Range(ValidationConstants.TicketRowNumberMinValue, ValidationConstants.TicketRowNumberMaxValue)]
        [JsonProperty("RowNumber")]
        public sbyte RowNumber { get; set; }

        [Required]
        [JsonProperty("PlayId")]
        public int PlayId { get; set; }
    }
}
