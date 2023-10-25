namespace ProductShop.DTOs.Export
{
    using System.Xml.Serialization;

    [XmlType("Users")]
    public class ExportFinalWrapperUserDto
    {
        [XmlElement("count")]
        public int TotalUsersCount { get; set; }

        [XmlArray("users")]
        public ExportWrappedUserDto[] Users { get; set; } = null!; 
    }
}
