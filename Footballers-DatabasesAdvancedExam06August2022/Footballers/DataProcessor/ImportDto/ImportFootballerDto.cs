namespace Footballers.DataProcessor.ImportDto
{
    using Footballers.Common;
    using System.ComponentModel.DataAnnotations;
    using System.Xml.Serialization;

    [XmlType("Footballer")]
    public class ImportFootballerDto
    {
        [Required]
        [MinLength(ValidationConstants.FootballerNameMinLength)]
        [MaxLength(ValidationConstants.FootballerNameMaxLength)]
        [XmlElement("Name")]
        public string Name { get; set; } = null!;

        [Required]
        [XmlElement("ContractStartDate")]
        public string ContractStartDate { get; set; } = null!;

        [Required]
        [XmlElement("ContractEndDate")]
        public string ContractEndDate { get; set; } = null!;

        [Required]
        [Range(ValidationConstants.BestSkillTypeMinValue, ValidationConstants.BestSkillTypeMaxValue)]
        [XmlElement("BestSkillType")]
        public int BestSkillType { get; set; }

        [Required]
        [Range(ValidationConstants.PositionTypeMinValue, ValidationConstants.PositionTypeMaxValue)]
        [XmlElement("PositionType")]
        public int PositionType { get; set; }
    }
}
