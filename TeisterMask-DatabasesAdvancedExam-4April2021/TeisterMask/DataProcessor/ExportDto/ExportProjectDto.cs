namespace TeisterMask.DataProcessor.ExportDto
{
	using System.Xml.Serialization;

	[XmlType("Project")]
	public class ExportProjectDto
	{
        public ExportProjectDto()
        {
            this.Tasks = new List<ExportTaskDto>();
        }

        [XmlAttribute("TasksCount")]
		public int TasksCount { get; set; }

		[XmlElement("ProjectName")]
		public string ProjectName { get; set; } = null!;

		[XmlElement("HasEndDate")]
		public string HasEndDate { get; set; } = null!;

		[XmlArray("Tasks")]
		public List<ExportTaskDto> Tasks { get; set; }
	}
}
