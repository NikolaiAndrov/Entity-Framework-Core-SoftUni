namespace Boardgames.DataProcessor.ExportDto
{
    using Newtonsoft.Json;

    public class ExportSellerDto
    {
        public ExportSellerDto()
        {
            this.Boardgames = new HashSet<ExportBoardgameDto>();
        }

        [JsonProperty("Name")]
        public string Name { get; set; } = null!;

        [JsonProperty("Website")]
        public string Website { get; set; } = null!;

        [JsonProperty("Boardgames")]
        public ICollection<ExportBoardgameDto> Boardgames { get; set; }
    }
}
