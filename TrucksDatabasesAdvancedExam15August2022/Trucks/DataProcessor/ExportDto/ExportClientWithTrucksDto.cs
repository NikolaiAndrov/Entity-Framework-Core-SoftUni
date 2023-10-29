namespace Trucks.DataProcessor.ExportDto
{
    using Newtonsoft.Json;

    public class ExportClientWithTrucksDto
    {
        [JsonProperty("Name")]
        public string Name { get; set; } = null!;

        [JsonProperty("Trucks")]
        public ICollection<ExportTruckDto> Trucks { get; set; } = null!;
    }
}
