namespace Footballers.DataProcessor.ImportDto
{
    using Footballers.Common;
    using System.ComponentModel.DataAnnotations;
    using System.Xml.Serialization;

    [XmlType("Coach")]
    public class ImportCoachDto
    {
        public ImportCoachDto()
        {
            this.Footballers = new HashSet<ImportFootballerDto>();
        }

        [Required]
        [MinLength(ValidationConstants.CoachNameMinLength)]
        [MaxLength(ValidationConstants.CoachNameMaxLength)]
        [XmlElement("Name")]
        public string Name { get; set; } = null!;

        [Required]
        [XmlElement("Nationality")]
        public string Nationality { get; set; } = null!;

        [XmlArray("Footballers")]
        public HashSet<ImportFootballerDto> Footballers { get; set;}
    }
}
