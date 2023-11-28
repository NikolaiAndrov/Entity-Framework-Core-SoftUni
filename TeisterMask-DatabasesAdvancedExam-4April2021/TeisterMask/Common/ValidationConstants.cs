namespace TeisterMask.Common
{
	public static class ValidationConstants
	{
		public static class EmployeeValidations
		{
			public const int UsernameMinLength = 3;
			public const int UsernameMaxLength = 40;
			public const string UsernameRegex = @"^[A-Za-z\d]{3,40}$";
			public const int PhoneLength = 12;
			public const string PhoneRegex = @"^\d{3}-\d{3}-\d{4}$";
		}

		public static class ProjectValidations
		{
			public const int NameMinLength = 2;
			public const int NameMaxLength = 40;
		}

		public static class TaskValidations
		{
			public const int NameMinLength = 2;
			public const int NameMaxLength = 40;
			public const int ExecutionTypeMinValue = 0;
			public const int ExecutionTypeMaxValue = 3;
			public const int LabelTypeMinValue = 0;
			public const int LabelTypeMaxValue = 4;
		}
	}
}
