namespace TeisterMask.DataProcessor.ImportDto
{
	using System.ComponentModel.DataAnnotations;
	using System.Xml.Serialization;
	using static Common.ValidationConstants.TaskValidations;

	[XmlType("Task")]
	public class ImportTaskDto
	{
		[Required]
		[StringLength(NameMaxLength, MinimumLength = NameMinLength)]
		[XmlElement("Name")]
		public string Name { get; set; } = null!;

		[Required]
		[XmlElement("OpenDate")]
		public string OpenDate { get; set; } = null!;

		[Required]
		[XmlElement("DueDate")]
		public string DueDate { get; set; } = null!;

		[Required]
		[Range(ExecutionTypeMinValue, ExecutionTypeMaxValue)]
		[XmlElement("ExecutionType")]
		public int ExecutionType { get; set; }

		[Required]
		[Range(LabelTypeMinValue, LabelTypeMaxValue)]
		[XmlElement("LabelType")]
		public int LabelType { get; set; }
	}
}
