namespace TeisterMask.Data.Models
{
	using System.ComponentModel.DataAnnotations;
	using static Common.ValidationConstants.EmployeeValidations;

	public class Employee
	{
        public Employee()
        {
            this.EmployeesTasks = new HashSet<EmployeeTask>();
        }

        [Key]
		public int Id { get; set; }

		[Required]
		[MaxLength(UsernameMaxLength)]
		public string Username { get; set; } = null!;

		[Required]
		public string Email { get; set; } = null!;

		[Required]
		[MaxLength(PhoneLength)]
		public string Phone { get; set; } = null!;

		public virtual ICollection<EmployeeTask> EmployeesTasks { get; set; }
	}
}
