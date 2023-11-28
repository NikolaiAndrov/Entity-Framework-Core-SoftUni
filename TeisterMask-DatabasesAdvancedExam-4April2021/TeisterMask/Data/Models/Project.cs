namespace TeisterMask.Data.Models
{
	using System.ComponentModel.DataAnnotations;
	using static Common.ValidationConstants.ProjectValidations;

	public class Project
	{
        public Project()
        {
            this.Tasks = new HashSet<Task>();
        }

        [Key]
		public int Id { get; set; }

		[Required]
		[MaxLength(NameMaxLength)]
		public string Name { get; set; } = null!;

		[Required]
		public DateTime OpenDate { get; set; }

		public DateTime? DueDate { get; set; }

		public ICollection<Task> Tasks { get; set; }
	}
}
