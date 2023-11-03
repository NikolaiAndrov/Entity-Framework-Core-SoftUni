namespace Artillery.DataProcessor.ImportDto
{

    using Newtonsoft.Json;
    using System.ComponentModel.DataAnnotations;

    public class ImportCountryIdDto
    {
        [Required]
        [JsonProperty("Id")]
        public int Id { get; set; }
    }
}
