namespace ProductShop.DTOs.Export
{
    using System.Xml.Serialization;

    [XmlType("SoldProducts")]
    public class ExportSoldProductsCountDto
    {
        [XmlElement("count")]
        public int SoldProductsCount { get; set; }

        [XmlArray("products")]
        public ExporWrappedSoldProductDto[] Products { get; set; } = null!;
    }
}
