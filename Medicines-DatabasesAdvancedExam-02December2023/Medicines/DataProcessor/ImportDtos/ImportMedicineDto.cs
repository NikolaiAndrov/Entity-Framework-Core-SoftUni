namespace Medicines.DataProcessor.ImportDtos
{
	using System.ComponentModel.DataAnnotations;
	using System.Xml.Serialization;

	[XmlType("Medicine")]
	public class ImportMedicineDto
	{
		[Required]
		[Range(0, 4)]
		[XmlAttribute("category")]
		public int Category {  get; set; }

		[Required]
		[StringLength(150, MinimumLength = 3)]
		[XmlElement("Name")]
		public string Name { get; set; } = null!;

		[Required]
		[Range(typeof(decimal), "0.01", "1000.00")]
		[XmlElement("Price")]
		public decimal Price { get; set; }

		[Required]
		[XmlElement("ProductionDate")]
		public string ProductionDate { get; set; } = null!;

		[Required]
		[XmlElement("ExpiryDate")]
		public string ExpiryDate { get; set; } = null!;

		[Required]
		[StringLength(100, MinimumLength = 3)]
		[XmlElement("Producer")]
		public string Producer {  get; set; } = null!;
	}
}
