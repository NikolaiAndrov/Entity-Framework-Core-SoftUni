namespace ProductShop.DTOs.Export
{
    using System.Xml.Serialization;

    [XmlType("User")]
    public class ExportUserWithSoldProductDto
    {
        public ExportUserWithSoldProductDto()
        {
        }

        [XmlElement("firstName")]
        public string FirstName { get; set; } = null!;

        [XmlElement("lastName")]
        public string? LastName { get; set; } = null!;

        [XmlArray("soldProducts")]
        public ExportSoldProductDto[] Products { get; set; }
    }
}
