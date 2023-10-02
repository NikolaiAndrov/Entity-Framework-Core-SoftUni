using Microsoft.EntityFrameworkCore;
using SoftUni.Data;
using SoftUni.Models;
using System.Globalization;
using System.Text;

namespace SoftUni
{
    public class StartUp
    {
        public static void Main(string[] args)
        {
            SoftUniContext context = new SoftUniContext();

            string result = RemoveTown(context);
            Console.WriteLine(result);
        }


        //P15
        public static string RemoveTown(SoftUniContext context)
        {
            var employeesToSetNullAddress = context.Employees
                .Where(e => e.Address.Town.Name == "Seattle");

            foreach (var address in employeesToSetNullAddress)
            {
                address.AddressId = null;
            }

            var townToDelete = context.Towns
                .FirstOrDefault(t => t.Name == "Seattle");

            var addresseToDelete = context.Addresses
                .Where(t => t.Town.Name == "Seattle")
                .ToArray();

            context.Addresses.RemoveRange(addresseToDelete);
            context.Towns.Remove(townToDelete);


            context.SaveChanges();
            return $"{addresseToDelete.Length} addresses in Seattle were deleted";
        }


        //P14
        public static string DeleteProjectById(SoftUniContext context)
        {
            var epToDelete = context.EmployeesProjects
                .Where(ep => ep.ProjectId == 2);

            var projectToDelete = context.Projects.Find(2);

            context.EmployeesProjects.RemoveRange(epToDelete);
            context.Projects.Remove(projectToDelete);
            context.SaveChanges();

            var projects = context
                .Projects
                .Take(10)
                .Select(p => new
                {
                    p.Name
                })
                .ToArray();

            StringBuilder sb = new StringBuilder();

            foreach (var project in projects)
            {
                sb.AppendLine(project.Name);
            }

            return sb.ToString().TrimEnd();
        }


        //P13
        public static string GetEmployeesByFirstNameStartingWithSa(SoftUniContext context)
        {
            var employees = context
                .Employees
                .Where(e => e.FirstName.StartsWith("Sa"))
                .Select(e => new
                {
                    e.FirstName,
                    e.LastName,
                    e.JobTitle,
                    e.Salary
                })
                .OrderBy(e => e.FirstName)
                .ThenBy(e => e.LastName)
                .ToArray();

            StringBuilder sb = new StringBuilder();

            foreach (var employee in employees)
            {
                sb.AppendLine($"{employee.FirstName} {employee.LastName} - {employee.JobTitle} - (${employee.Salary:f2})");
            }

            return sb.ToString().TrimEnd();
        }


        //P12
        public static string IncreaseSalaries(SoftUniContext context)
        {
            var employees = context
                .Employees
                .Where(e => e.Department.Name == "Engineering"
                    || e.Department.Name == "Tool Design" || e.Department.Name == "Marketing"
                    || e.Department.Name == "Information Services")
                .Select(e => new
                {
                    Salary = Decimal.Multiply(e.Salary, 1.12m),
                    e.FirstName,
                    e.LastName
                })
                .OrderBy(e => e.FirstName)
                .ThenBy(e => e.LastName)
                .ToArray();

            StringBuilder sb = new StringBuilder();

            foreach (var employee in employees)
            {
                sb.AppendLine($"{employee.FirstName} {employee.LastName} (${employee.Salary:f2})");
            }

            return sb.ToString().TrimEnd();
        }


        // P11
        public static string GetLatestProjects(SoftUniContext context)
        {
            var projects = context
                .Projects
                .OrderByDescending(p => p.StartDate)
                .Take(10)
                .Select(p => new
                {
                    p.Name,
                    p.Description,
                    StartDate = p.StartDate.ToString("M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture)
                })
                .OrderBy(p => p.Name)
                .ToArray();

            StringBuilder sb = new StringBuilder();

            foreach (var project in projects)
            {
                sb.AppendLine(project.Name);
                sb.AppendLine(project.Description);
                sb.AppendLine(project.StartDate);
            }

            return sb.ToString().TrimEnd();
        }


        //P10
        public static string GetDepartmentsWithMoreThan5Employees(SoftUniContext context)
        {
            var departmentsEmployees = context
                .Departments
                .Where(d => d.Employees.Count > 5)
                .OrderBy(d => d.Employees.Count)
                .ThenBy(d => d.Name)
                .Select(d => new
                {
                    DepartmentName = d.Name,
                    ManagaerFirstName = d.Manager.FirstName,
                    ManagerLastName = d.Manager.LastName,
                    Employees =
                        d.Employees
                        .OrderBy(e => e.FirstName)
                        .ThenBy(e => e.LastName)
                        .Select(e => new
                        {
                            e.FirstName,
                            e.LastName,
                            e.JobTitle
                        })
                        .ToArray()

                })
                .ToArray();

            StringBuilder sb = new StringBuilder();

            foreach (var department in departmentsEmployees)
            {
                sb.AppendLine($"{department.DepartmentName} – {department.ManagaerFirstName} {department.ManagerLastName}");

                foreach (var employee in department.Employees)
                {
                    sb.AppendLine($"{employee.FirstName} {employee.LastName} - {employee.JobTitle}");
                }
            }

            return sb.ToString().TrimEnd();
        }


        //P09
        public static string GetEmployee147(SoftUniContext context)
        {
            var employeeProjects = context.Employees
                .Where(e => e.EmployeeId == 147)
                .Select(e => new
                {
                    e.FirstName,
                    e.LastName,
                    e.JobTitle,
                    Projects = e.EmployeesProjects
                    .OrderBy(p => p.Project.Name)
                    .Select(p => p.Project.Name)
                    .ToArray()

                })
                .ToArray();

            StringBuilder sb = new StringBuilder();

            foreach (var employee in employeeProjects)
            {
                sb.AppendLine($"{employee.FirstName} {employee.LastName} - {employee.JobTitle}");

                foreach (var project in employee.Projects)
                {
                    sb.AppendLine(project);
                }
            }

            return sb.ToString().TrimEnd();
        }


        //P08
        public static string GetAddressesByTown(SoftUniContext context)
        {
            var addresses = context
                .Addresses
                .OrderByDescending(a => a.Employees.Count)
                .ThenBy(a => a.Town.Name)
                .Take(10)
                .Select(a => new
                {
                    a.AddressText,
                    TownName = a.Town.Name,
                    EmployeesCount = a.Employees.Count,
                })
                .ToArray();

            StringBuilder sb = new StringBuilder();

            foreach (var address in addresses)
            {
                sb.AppendLine($"{address.AddressText}, {address.TownName} - {address.EmployeesCount} employees");
            }

            return sb.ToString().TrimEnd();
        }


        //P07
        public static string GetEmployeesInPeriod(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();

            var employeesWithProjects = context.Employees
                .Take(10)
                .Select(e => new
                {
                    e.FirstName,
                    e.LastName,
                    ManagerFirstName = e.Manager!.FirstName,
                    ManagerLastName = e.Manager!.LastName,
                    Projects = e.EmployeesProjects
                        .Where(ep => ep.Project.StartDate.Year >= 2001 && ep.Project.StartDate.Year <= 2003)
                        .Select(ep => new
                        {
                            ProjectName = ep.Project.Name,
                            StartDate = ep.Project.StartDate.ToString("M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture),
                            EndDate = ep.Project.EndDate.HasValue
                                ? ep.Project.EndDate.Value.ToString("M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture)
                                : "not finished"
                        })
                        .ToArray()
                })
                .ToArray();

            foreach (var e in employeesWithProjects)
            {
                sb.AppendLine($"{e.FirstName} {e.LastName} - Manager: {e.ManagerFirstName} {e.ManagerLastName}");

                foreach (var p in e.Projects)
                {
                    sb.AppendLine($"--{p.ProjectName} - {p.StartDate} - {p.EndDate}");
                }
            }

            return sb.ToString().TrimEnd();
        }


        //P06
        public static string AddNewAddressToEmployee(SoftUniContext context)
        {
            var employee = context
                .Employees
                .FirstOrDefault(e => e.LastName == "Nakov");

            var address = new Address();
            address.AddressText = "Vitoshka 15";
            address.TownId = 4;

            if (employee != null)
            {
                employee.Address = address;
                context.SaveChanges();
            }

            var employees = context
                .Employees
                .AsNoTracking()
                .OrderByDescending(e => e.AddressId)
                .Take(10)
                .Select(e => new
                {
                    e.Address.AddressText
                })
                .ToArray();

            StringBuilder sb = new StringBuilder();

            foreach (var item in employees)
            {
                sb.AppendLine(item.AddressText);
            }

            return sb.ToString().TrimEnd();
        }


        //P05
        public static string GetEmployeesFromResearchAndDevelopment(SoftUniContext context)
        {
            var employees = context
                .Employees
                .AsNoTracking()
                .Where(e => e.Department.Name == "Research and Development")
                .OrderBy(e => e.Salary)
                .ThenByDescending(e => e.FirstName)
                .Select(e => new
                {
                    e.FirstName,
                    e.LastName,
                    DepartmentName = e.Department.Name,
                    e.Salary
                })
                .ToArray();

            StringBuilder sb = new StringBuilder();

            foreach (var employee in employees)
            {
                sb.AppendLine($"{employee.FirstName} {employee.LastName} from {employee.DepartmentName} - ${employee.Salary:f2}");
            }

            return sb.ToString().TrimEnd();
        }


        //P04
        public static string GetEmployeesWithSalaryOver50000(SoftUniContext context)
        {
            var employees = context
                .Employees
                .AsNoTracking()
                .Where(e => e.Salary > 50000)
                .OrderBy(e => e.FirstName)
                .Select(e => new
                {
                    e.FirstName,
                    e.Salary
                })
                .ToArray();

            StringBuilder sb = new StringBuilder();

            foreach (var employee in employees)
            {
                sb.AppendLine($"{employee.FirstName} - {employee.Salary:f2}");
            }

            return sb.ToString().TrimEnd();
        }


        //P03
        public static string GetEmployeesFullInformation(SoftUniContext context)
        {
            var employees = context
                .Employees
                .AsNoTracking()
                .OrderBy(e => e.EmployeeId)
                .Select(e => new
                {
                    e.FirstName,
                    e.LastName,
                    e.MiddleName,
                    e.JobTitle,
                    e.Salary
                })
                .ToArray();

            StringBuilder sb = new StringBuilder();

            foreach (var employee in employees)
            {
                sb.AppendLine($"{employee.FirstName} {employee.LastName} {employee.MiddleName} {employee.JobTitle} {employee.Salary:f2}");
            }

            return sb.ToString().TrimEnd();
        }
    }
}