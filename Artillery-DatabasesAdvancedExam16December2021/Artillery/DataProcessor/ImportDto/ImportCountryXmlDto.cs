namespace Artillery.DataProcessor.ImportDto
{
    using Artillery.Common;
    using System.ComponentModel.DataAnnotations;
    using System.Xml.Serialization;

    [XmlType("Country")]
    public class ImportCountryXmlDto
    {
        [Required]
        [MinLength(ValidationConstants.CountryNameMinLength)]
        [MaxLength(ValidationConstants.CountryNameMaxLength)]
        [XmlElement("CountryName")]
        public string CountryName { get; set; } = null!;

        [Required]
        [Range(ValidationConstants.ArmySizeMinValue, ValidationConstants.ArmySizeMaxValue)]
        [XmlElement("ArmySize")]
        public int ArmySize { get; set; }
    }
}
