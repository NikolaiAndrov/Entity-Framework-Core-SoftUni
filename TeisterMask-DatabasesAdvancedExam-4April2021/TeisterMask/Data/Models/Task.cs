namespace TeisterMask.Data.Models
{
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;
	using TeisterMask.Data.Models.Enums;
	using static Common.ValidationConstants.TaskValidations;

	public class Task
	{
        public Task()
        {
            this.EmployeesTasks = new HashSet<EmployeeTask>();
        }

        [Key]
		public int Id { get; set; }

		[Required]
		[MaxLength(NameMaxLength)]
		public string Name { get; set; } = null!;

		[Required]
		public DateTime OpenDate { get; set; }

		[Required]
		public DateTime DueDate { get; set; }

		[Required]
		public ExecutionType ExecutionType { get; set; }

		[Required]
		public LabelType LabelType { get; set; }

		[Required]
		public int ProjectId { get; set; }

		[Required]
		[ForeignKey(nameof(ProjectId))]
		public virtual Project Project { get; set; } = null!;

		public virtual ICollection<EmployeeTask> EmployeesTasks { get; set; }
	}
}
