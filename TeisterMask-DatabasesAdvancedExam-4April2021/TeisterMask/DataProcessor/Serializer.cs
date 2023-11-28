namespace TeisterMask.DataProcessor
{
    using Data;
	using Newtonsoft.Json;
	using System.Globalization;
	using TeisterMask.DataProcessor.ExportDto;
	using TeisterMask.Utilities;

	public class Serializer
    {
        public static string ExportProjectWithTheirTasks(TeisterMaskContext context)
        {
            XmlParser xmlParser = new XmlParser();

            var projects = context.Projects
                .Where(p => p.Tasks.Any())
                .Select(p => new ExportProjectDto
                {
                    TasksCount = p.Tasks.Count(),
                    ProjectName = p.Name,
                    HasEndDate = p.DueDate.HasValue ? "Yes" : "No",
                    Tasks = p.Tasks
                    .Select(t => new ExportTaskDto
                    {
                        Name = t.Name,
                        Label = t.LabelType.ToString(),
                    })
                    .OrderBy(t => t.Name)
                    .ToList()
				})
                .OrderByDescending(p => p.TasksCount)
                .ThenBy(p => p.ProjectName)
                .ToList();

            return xmlParser.Serialize(projects, "Projects");
        }

        public static string ExportMostBusiestEmployees(TeisterMaskContext context, DateTime date)
        {
            var employees = context.Employees
                .Where(e => e.EmployeesTasks.Any(t => t.Task.OpenDate >= date))
                .Select(e => new
                {
                    e.Username,
                    Tasks = e.EmployeesTasks
                    .Where(t => t.Task.OpenDate >= date)
                    .OrderByDescending(t => t.Task.DueDate)
                    .ThenBy(t => t.Task.Name)
                    .Select(t => new
                    {
                        TaskName = t.Task.Name,
                        OpenDate = t.Task.OpenDate.ToString("d", CultureInfo.InvariantCulture),
                        DueDate = t.Task.DueDate.ToString("d", CultureInfo.InvariantCulture),
                        LabelType = t.Task.LabelType.ToString(),
                        ExecutionType = t.Task.ExecutionType.ToString(),
                    })
                    .ToArray()
                })
                .OrderByDescending(e => e.Tasks.Count())
                .ThenBy(e => e.Username)
                .Take(10)
                .ToArray();

            return JsonConvert.SerializeObject(employees, Formatting.Indented);
		}
    }
}