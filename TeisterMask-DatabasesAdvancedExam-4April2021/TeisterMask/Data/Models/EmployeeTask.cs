namespace TeisterMask.Data.Models
{
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;

	public class EmployeeTask
	{
		[Required]
		public int EmployeeId { get; set; }

		[Required]
		[ForeignKey(nameof(EmployeeId))]
		public virtual Employee Employee { get; set; } = null!;

		[Required]
		public int TaskId { get; set; }

		[Required]
		[ForeignKey(nameof(TaskId))]
		public virtual Task Task { get; set; } = null!;
	}
}
