namespace Invoices.DataProcessor.ImportDto
{
    using Invoices.Common;
    using System.ComponentModel.DataAnnotations;
    using System.Xml.Serialization;

    [XmlType("Client")]
    public class ImportClientDto
    {
        public ImportClientDto()
        {
            this.Address = new HashSet<ImportAddressDto>();
        }

        [Required]
        [MinLength(ValidationConstants.ClientNameMinLength)]
        [MaxLength(ValidationConstants.ClientNameMaxLength)]
        [XmlElement("Name")]
        public string Name { get; set; } = null!;

        [Required]
        [MinLength(ValidationConstants.ClientNumberVatMinLength)]
        [MaxLength(ValidationConstants.ClientNumberVatMaxLength)]
        [XmlElement("NumberVat")]
        public string NumberVat { get; set; } = null!;

        [XmlArray("Addresses")]
        public HashSet<ImportAddressDto> Address { get; set; }
    }
}
