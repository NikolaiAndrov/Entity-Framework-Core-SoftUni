namespace TeisterMask.DataProcessor.ImportDto
{
	using System.ComponentModel.DataAnnotations;
	using System.Xml.Serialization;
	using static Common.ValidationConstants.ProjectValidations;

	[XmlType("Project")]
	public class ImportProjectDto
	{
        public ImportProjectDto()
        {
			this.Tasks = new List<ImportTaskDto>();
        }

		[Required]
		[StringLength(NameMaxLength, MinimumLength = NameMinLength)]
		[XmlElement("Name")]
		public string Name { get; set; } = null!;

		[Required]
		[XmlElement("OpenDate")]
		public string OpenDate { get; set; } = null!;

		[XmlElement("DueDate")]
		public string? DueDate { get; set; }

		[XmlArray("Tasks")]
		public List<ImportTaskDto> Tasks { get; set; }
	}
}
