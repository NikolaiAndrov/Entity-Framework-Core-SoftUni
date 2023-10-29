namespace Trucks.DataProcessor.ExportDto
{
    using Trucks.Data.Models.Enums;
    using Newtonsoft.Json;

    public class ExportTruckDto
    {
        [JsonProperty("TruckRegistrationNumber")]
        public string? TruckRegistrationNumber { get; set; }

        [JsonProperty("VinNumber")]
        public string VinNumber { get; set; } = null!;

        [JsonProperty("TankCapacity")]
        public int TankCapacity { get; set; }

        [JsonProperty("CargoCapacity")]
        public int CargoCapacity { get; set; }

        [JsonProperty("CategoryType")]
        public string CategoryType { get; set; } = null!;

        [JsonProperty("MakeType")]
        public string MakeType { get; set; } = null!;
    }
}
