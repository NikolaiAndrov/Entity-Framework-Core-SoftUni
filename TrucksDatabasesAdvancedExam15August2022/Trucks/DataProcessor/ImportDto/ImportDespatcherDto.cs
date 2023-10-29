namespace Trucks.DataProcessor.ImportDto
{
    using System.ComponentModel.DataAnnotations;
    using System.Xml.Serialization;
    using Trucks.Common;

    [XmlType("Despatcher")]
    public class ImportDespatcherDto
    {
        public ImportDespatcherDto()
        {
            this.Trucks = new List<ImportTruckDto>();
        }

        [Required]
        [MinLength(ValidationConstants.DespatcherNameMinLength)]
        [MaxLength(ValidationConstants.DespatcherNameMaxLength)]
        [XmlElement("Name")]
        public string Name { get; set; } = null!;

        [Required]
        [XmlElement("Position")]
        public string Position { get; set; }

        [XmlArray("Trucks")]
        public List<ImportTruckDto> Trucks { get; set; }
    }
}
