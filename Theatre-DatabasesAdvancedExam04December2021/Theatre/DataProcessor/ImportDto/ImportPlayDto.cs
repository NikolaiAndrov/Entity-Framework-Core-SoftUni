namespace Theatre.DataProcessor.ImportDto
{
    using System.ComponentModel.DataAnnotations;
    using Theatre.Common;
    using System.Xml.Serialization;

    [XmlType("Play")]
    public class ImportPlayDto
    {
        [Required]
        [MinLength(ValidationConstants.PlayTitleMinLength)]
        [MaxLength(ValidationConstants.PlayTitleMaxLength)]
        [XmlElement("Title")]
        public string Title { get; set; } = null!;

        [Required]
        [XmlElement("Duration")]
        public string Duration { get; set; } = null!;

        [Required]
        [Range(ValidationConstants.PlayRatingMinValue, ValidationConstants.PlayRatingMaxValue)]
        [XmlElement("Raiting")]
        public float Rating { get; set; }

        [Required]
        [XmlElement("Genre")]
        public string Genre { get; set; } = null!;

        [Required]
        [MaxLength(ValidationConstants.PlayDescriptionMaxLength)]
        [XmlElement("Description")]
        public string Description { get; set; } = null!;

        [Required]
        [MinLength(ValidationConstants.PlayScreenwriterMinLength)]
        [MaxLength(ValidationConstants.PlayScreenwriterMaxLength)]
        [XmlElement("Screenwriter")]
        public string Screenwriter { get; set; } = null!;
    }
}
