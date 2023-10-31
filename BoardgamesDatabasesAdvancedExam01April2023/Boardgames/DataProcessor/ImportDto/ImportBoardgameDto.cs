namespace Boardgames.DataProcessor.ImportDto
{
    using Boardgames.Common;
    using System.ComponentModel.DataAnnotations;
    using System.Xml.Serialization;

    [XmlType("Boardgame")]
    public class ImportBoardgameDto
    {
        [Required]
        [MinLength(ValidationConstants.BoardGameNameMinLength)]
        [MaxLength(ValidationConstants.BoardGameNameMaxLength)]
        [XmlElement("Name")]
        public string Name { get; set; } = null!;

        [Required]
        [Range(ValidationConstants.BoardGameRatingMinValue, 
            ValidationConstants.BoardGameRatingMaxValue)]
        [XmlElement("Rating")]
        public double Rating { get; set; }

        [Required]
        [Range(ValidationConstants.BoardGameYearPublishedMinValue, 
            ValidationConstants.BoardGameYearPublishedMaxValue)]
        [XmlElement("YearPublished")]
        public int YearPublished { get; set; }

        [Required]
        [Range(ValidationConstants.BoardGameCategoryTypeMinValue,
            ValidationConstants.BoardGameCategoryTypeMaxValue)]
        [XmlElement("CategoryType")]
        public int CategoryType { get; set; }

        [Required]
        [XmlElement("Mechanics")]
        public string Mechanics { get; set; } = null!;
    }
}
