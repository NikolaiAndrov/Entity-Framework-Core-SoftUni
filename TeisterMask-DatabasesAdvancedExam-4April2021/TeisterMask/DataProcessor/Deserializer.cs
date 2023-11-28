// ReSharper disable InconsistentNaming

namespace TeisterMask.DataProcessor
{
    using System.ComponentModel.DataAnnotations;
	using TeisterMask.DataProcessor.ImportDto;
    using ValidationContext = System.ComponentModel.DataAnnotations.ValidationContext;

    using Data;
	using System.Text;
	using TeisterMask.Utilities;
	using TeisterMask.Data.Models;
	using System.Globalization;
	using TeisterMask.Data.Models.Enums;
	using Newtonsoft.Json;

	public class Deserializer
    {
        private const string ErrorMessage = "Invalid data!";

        private const string SuccessfullyImportedProject
            = "Successfully imported project - {0} with {1} tasks.";

        private const string SuccessfullyImportedEmployee
            = "Successfully imported employee - {0} with {1} tasks.";

        public static string ImportProjects(TeisterMaskContext context, string xmlString)
        {
            XmlParser xmlParser = new XmlParser();
            StringBuilder sb = new StringBuilder();

            ImportProjectDto[] importProjectDtos = xmlParser.Deserialize<ImportProjectDto[]>(xmlString, "Projects");

            ICollection<Project> validProjects = new HashSet<Project>();

            foreach (var projectDto in importProjectDtos)
            {
                if (!IsValid(projectDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                string dateFormat = "dd/MM/yyyy";

                if (!DateTime.TryParseExact(projectDto.OpenDate, dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime openDate))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                DateTime? dueDate;

                try
                {
                    dueDate = DateTime.ParseExact(projectDto.DueDate, dateFormat, CultureInfo.InvariantCulture);
                }
                catch (Exception)
                {
                    dueDate = null;
                }

                Project project = new Project
                {
                    Name = projectDto.Name,
                    OpenDate = openDate,
                    DueDate = dueDate,
                };

                foreach (var taskDto in projectDto.Tasks)
                {
                    DateTime taskOpenDate;
                    DateTime taskDueDate;

                    try
                    {
                        taskOpenDate = DateTime.ParseExact(taskDto.OpenDate, dateFormat, CultureInfo.InvariantCulture);
                        taskDueDate = DateTime.ParseExact(taskDto.DueDate, dateFormat, CultureInfo.InvariantCulture);
                    }
                    catch (Exception)
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

					if (!IsValid(taskDto) || 
                        taskOpenDate > taskDueDate || 
                        taskOpenDate < openDate ||
                        dueDate.HasValue && taskDueDate > dueDate)
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    Task task = new Task
                    {
                        Name = taskDto.Name,
                        OpenDate = taskOpenDate,
                        DueDate = taskDueDate,
                        ExecutionType = (ExecutionType)taskDto.ExecutionType,
                        LabelType = (LabelType)taskDto.LabelType
                    };

                    project.Tasks.Add(task);
                }

                validProjects.Add(project);
                sb.AppendLine(string.Format(SuccessfullyImportedProject, project.Name, project.Tasks.Count));
            }

            context.Projects.AddRange(validProjects);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportEmployees(TeisterMaskContext context, string jsonString)
        {
			StringBuilder sb = new StringBuilder();

            ICollection<int> existingTasks = context
                .Tasks
                .Select(t => t.Id)
                .ToHashSet();

            ICollection<Employee> validEmployees = new HashSet<Employee>();

            ImportEmployeeDto[] importEmployeeDtos = JsonConvert.DeserializeObject<ImportEmployeeDto[]>(jsonString)!;

            foreach (var employeeDto in importEmployeeDtos)
            {
                if (!IsValid(employeeDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Employee employee = new Employee
                {
                    Username = employeeDto.Username,
                    Email = employeeDto.Email,
                    Phone = employeeDto.Phone
                };

                foreach (var taskId in employeeDto.Tasks)
                {
                    if (!existingTasks.Contains(taskId))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    EmployeeTask employeeTask = new EmployeeTask
                    {
                        Employee = employee,
                        TaskId = taskId
                    };

                    employee.EmployeesTasks.Add(employeeTask);
                }

                validEmployees.Add(employee);
                sb.AppendLine(string.Format(SuccessfullyImportedEmployee, employee.Username, employee.EmployeesTasks.Count));
            }

            context.Employees.AddRange(validEmployees);
            context.SaveChanges();

			return sb.ToString().TrimEnd();
		}

        private static bool IsValid(object dto)
        {
            var validationContext = new ValidationContext(dto);
            var validationResult = new List<ValidationResult>();

            return Validator.TryValidateObject(dto, validationContext, validationResult, true);
        }
    }
}