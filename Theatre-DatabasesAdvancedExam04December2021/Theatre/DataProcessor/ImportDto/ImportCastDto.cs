namespace Theatre.DataProcessor.ImportDto
{
    using System.ComponentModel.DataAnnotations;
    using System.Xml.Serialization;
    using Theatre.Common;

    [XmlType("Cast")]
    public class ImportCastDto
    {
        [Required]
        [MinLength(ValidationConstants.CastFullNameMinLength)]
        [MaxLength(ValidationConstants.CastFullNameMaxLength)]
        [XmlElement("FullName")]
        public string FullName { get; set; } = null!;

        [Required]
        [XmlElement("IsMainCharacter")]
        public string IsMainCharacter { get; set; } = null!;

        [Required]
        [MinLength(ValidationConstants.CastPhoneNumberLength)]
        [MaxLength(ValidationConstants.CastPhoneNumberLength)]
        [RegularExpression(ValidationConstants.CastPhoneNumberRegex)]
        [XmlElement("PhoneNumber")]
        public string PhoneNumber { get; set; } = null!;

        [Required]
        [XmlElement("PlayId")]
        public int PlayId { get; set; }
    }
}
