namespace Boardgames.DataProcessor.ExportDto
{
    using System.Xml.Serialization;

    [XmlType("Creator")]
    public class ExportCreatorXmlDto
    {
        public ExportCreatorXmlDto()
        {
            Boeardgames = new List<ExportBoeardgameXmlDto>();
        }

        [XmlAttribute("BoardgamesCount")]
        public int BoardgamesCount { get; set; }

        [XmlElement("CreatorName")]
        public string CreatorName { get; set; } = null!;

        [XmlArray("Boardgames")]
        public List<ExportBoeardgameXmlDto> Boeardgames { get; set; }
    }
}
