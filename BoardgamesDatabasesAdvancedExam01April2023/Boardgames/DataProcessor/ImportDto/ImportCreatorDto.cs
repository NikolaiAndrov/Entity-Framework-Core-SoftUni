namespace Boardgames.DataProcessor.ImportDto
{
    using Boardgames.Common;
    using System.ComponentModel.DataAnnotations;
    using System.Xml.Serialization;

    [XmlType("Creator")]
    public class ImportCreatorDto
    {
        public ImportCreatorDto()
        {
            this.Boardgames = new HashSet<ImportBoardgameDto>();
        }

        [Required]
        [MinLength(ValidationConstants.CreatorFirstNameMinLength)]
        [MaxLength(ValidationConstants.CreatorFirstNameMaxLength)]
        [XmlElement("FirstName")]
        public string FirstName { get; set; } = null!;

        [Required]
        [MinLength(ValidationConstants.CreatorLastNameMinLength)]
        [MaxLength(ValidationConstants.CreatorLastNameMaxLength)]
        [XmlElement("LastName")]
        public string LastName { get; set; } = null!;

        [XmlArray("Boardgames")]
        public HashSet<ImportBoardgameDto> Boardgames { get; set; }
    }
}
