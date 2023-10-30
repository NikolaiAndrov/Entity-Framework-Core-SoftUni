namespace Invoices.DataProcessor.ImportDto
{
    using Invoices.Common;
    using System.ComponentModel.DataAnnotations;
    using System.Xml.Serialization;

    [XmlType("Address")]
    public class ImportAddressDto
    {
        [Required]
        [MinLength(ValidationConstants.StreetNameMinLength)]
        [MaxLength(ValidationConstants.StreetNameMaxLength)]
        [XmlElement("StreetName")]
        public string StreetName { get; set; } = null!;

        [Required]
        [XmlElement("StreetNumber")]
        public int StreetNumber { get; set; }

        [Required]
        [XmlElement("PostCode")]
        public string PostCode { get; set; } = null!;

        [Required]
        [MinLength(ValidationConstants.CityNameMinLength)]
        [MaxLength(ValidationConstants.CityNameMaxLength)]
        [XmlElement("City")]
        public string City { get; set; } = null!;

        [Required]
        [MinLength(ValidationConstants.CountryNameMinLength)]
        [MaxLength(ValidationConstants.CountryNameMaxLength)]
        [XmlElement("Country")]
        public string Country { get; set; } = null!;
    }
}
