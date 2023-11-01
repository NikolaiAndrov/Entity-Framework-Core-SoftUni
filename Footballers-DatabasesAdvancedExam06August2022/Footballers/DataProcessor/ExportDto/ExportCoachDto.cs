namespace Footballers.DataProcessor.ExportDto
{
    using System.Xml.Serialization;

    [XmlType("Coach")]
    public class ExportCoachDto
    {
        public ExportCoachDto()
        {
            this.Footballers = new HashSet<ExportFootballerDto>();
        }

        [XmlAttribute("FootballersCount")]
        public int FootballersCount { get; set; }

        [XmlElement("CoachName")]
        public string Name { get; set; } = null!;

        [XmlArray("Footballers")]
        public HashSet<ExportFootballerDto> Footballers { get; set;}
    }
}
