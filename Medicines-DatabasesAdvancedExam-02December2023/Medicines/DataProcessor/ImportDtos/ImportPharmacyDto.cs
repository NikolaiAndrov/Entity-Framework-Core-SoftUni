namespace Medicines.DataProcessor.ImportDtos
{
	using System.ComponentModel.DataAnnotations;
	using System.Xml.Serialization;

	[XmlType("Pharmacy")]
	public class ImportPharmacyDto
	{
        public ImportPharmacyDto()
        {
			this.Medicines = new List<ImportMedicineDto>();
        }

		[Required]
		[XmlAttribute("non-stop")]
        public string IsNonStop { get; set; } = null!;

		[Required]
		[StringLength(50, MinimumLength = 2)]
		[XmlElement("Name")]
		public string Name { get; set; } = null!;

		[Required]
		[StringLength(14, MinimumLength = 14)]
		[RegularExpression(@"^\(\d{3}\)\s\d{3}-\d{4}$")]
		[XmlElement("PhoneNumber")]
		public string PhoneNumber { get; set; } = null!;

		[XmlArray("Medicines")]
		public List<ImportMedicineDto> Medicines { get; set; }
	}
}
