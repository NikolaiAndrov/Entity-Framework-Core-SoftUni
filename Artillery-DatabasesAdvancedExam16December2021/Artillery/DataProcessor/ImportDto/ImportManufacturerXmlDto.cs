namespace Artillery.DataProcessor.ImportDto
{
    using Artillery.Common;
    using System.ComponentModel.DataAnnotations;
    using System.Xml.Serialization;

    [XmlType("Manufacturer")]
    public class ImportManufacturerXmlDto
    {
        [Required]
        [MinLength(ValidationConstants.ManufacturerNameMinLength)]
        [MaxLength(ValidationConstants.ManufacturerNameMaxLength)]
        [XmlElement("ManufacturerName")]
        public string ManufacturerName { get; set; } = null!;

        [Required]
        [MinLength(ValidationConstants.FoundedMinLength)]
        [MaxLength(ValidationConstants.FoundedMaxLength)]
        [XmlElement("Founded")]
        public string Founded { get; set; } = null!;
    }
}
