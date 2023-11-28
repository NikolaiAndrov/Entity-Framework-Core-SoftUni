namespace TeisterMask.DataProcessor.ImportDto
{
	using static Common.ValidationConstants.EmployeeValidations;

	using System.ComponentModel.DataAnnotations;

	public class ImportEmployeeDto
	{
        public ImportEmployeeDto()
        {
            this.Tasks = new HashSet<int>();
        }

		[Required]
		[StringLength(UsernameMaxLength, MinimumLength = UsernameMinLength)]
		[RegularExpression(UsernameRegex)]
		public string Username { get; set; } = null!;

		[Required]
		[EmailAddress]
		public string Email { get; set; } = null!;

		[Required]
		[StringLength(PhoneLength, MinimumLength = PhoneLength)]
		[RegularExpression(PhoneRegex)]
		public string Phone { get; set; } = null!;

		public ICollection<int> Tasks { get; set; }
	}
}
