using Microsoft.EntityFrameworkCore;
using SoftUni.Data;
using SoftUni.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace SoftUni
{
    public class StartUp
    {
        public static void Main(string[] args)
        {
            var SUcontext = new SoftUniContext();
            string result = GetEmployeesByFirstNameStartingWithSa(SUcontext);
            Console.WriteLine(result);

        }

        public static string RemoveTown(SoftUniContext context)
        {
            var seattle = context.Towns
                .Where(x => x.Name == "Seattle")
                .FirstOrDefault();

            var addressesInSeattle = context.Addresses
                .Where(x => x.TownId == seattle.TownId)
                .ToList();

            var addressesInSeattleList = addressesInSeattle.Select(x => x.AddressId).ToList();

            var employeesInSeattle = context.Employees
                .Where(x => x.AddressId != null && addressesInSeattleList.Contains(x.AddressId.Value));

            foreach (var employee in employeesInSeattle)
            {
                context.Remove(employee);
            }

            foreach (var address in addressesInSeattle)
            {
                context.Remove(address);
            }

            context.Remove(seattle);

            context.SaveChanges();

            return $"{addressesInSeattle.Count} addresses in Seattle were deleted";
        }

        public static string DeleteProjectById(SoftUniContext context)
        {
            var project = context.Projects
                .Where(x => x.ProjectId == 2)
                .FirstOrDefault();

            var employeeProjects = context.EmployeesProjects
                .Where(x => x.ProjectId == project.ProjectId);

            foreach (var employeeProject in employeeProjects)
            {
                context.Remove(employeeProject);
            }

            context.Remove(project);

            context.SaveChanges();


            var projects = context.Projects
                .Take(10)
                .Select(x => new
                {
                    x.Name,
                })
                .ToList();

            var sb = new StringBuilder();
            foreach (var currProject in projects)
            {
                sb.AppendLine(currProject.Name);
            }

            return sb.ToString().TrimEnd();
        }

        public static string GetEmployeesByFirstNameStartingWithSa(SoftUniContext context)
        {
            var employees = context.Employees
                .Where(x => x.FirstName.StartsWith("Sa"))
                .Select(x => new
                {
                    x.FirstName,
                    x.LastName,
                    x.JobTitle,
                    x.Salary,
                })
                .OrderBy(x => x.FirstName)
                .ThenBy(x => x.LastName)
                .ToList();

            var sb = new StringBuilder();

            foreach (var employee in employees)
            {
                sb.AppendLine($"{employee.FirstName} {employee.LastName} - {employee.JobTitle} - (${employee.Salary:F2})");
            }

            return sb.ToString().TrimEnd();
        }

        public static string IncreaseSalaries(SoftUniContext context)
        {
            //Engineering, Tool Design, Marketing, Information Services
            List<string> departments = new List<string>() { "Engineering", "Tool Design", "Marketing", "Information Services" };
            var employees = context.Employees
                .Where(x => departments.Contains(x.Department.Name));
                

            foreach (var employee in employees)
            {
                employee.Salary *= 1.12m;
            }

            var sb = new StringBuilder();

            foreach (var employee in employees.OrderBy(x => x.FirstName).ThenBy(x => x.LastName).ToList())
            {
                sb.AppendLine($"{employee.FirstName} {employee.LastName} (${employee.Salary:F2})");
            }

            return sb.ToString().TrimEnd();
        }

        public static string GetLatestProjects(SoftUniContext context) // not done somehow
        {
            var projects = context.Projects
                .OrderByDescending(x => x.StartDate)
                .Take(10)
                .OrderBy(x => x.Name)
                .Select(x => new
                {
                    x.Name,
                    x.Description,
                    x.StartDate
                });
                


            var sb = new StringBuilder();
            foreach (var project in projects)
            {
                sb.AppendLine($"{project.Name}");
                sb.AppendLine($"{project.Description}");
                sb.AppendLine($"{project.StartDate.ToString("M/d/yyyy h:mm:ss tt")}");
            }

            return sb.ToString();
        }

        public static string GetDepartmentsWithMoreThan5Employees(SoftUniContext context)
        {
            var departments = context.Departments
                .Where(x => x.Employees.Count > 5)
                .OrderBy(x => x.Employees.Count)
                .ThenBy(x => x.Name)
                .Select(x => new
                {
                    x.Name,
                    ManagerFirstName = x.Manager.FirstName,
                    ManagerLastName = x.Manager.LastName,
                    Employees = x.Employees.Select(e => new
                        {
                            e.FirstName,
                            e.LastName,
                            e.JobTitle,
                        })
                        .OrderBy(e => e.FirstName)
                        .ThenBy(e => e.LastName)
                        .ToList(),
                })
                .ToList();

            var sb = new StringBuilder();

            foreach (var department in departments)
            {
                sb.AppendLine($"{department.Name} - {department.ManagerFirstName} {department.ManagerLastName}");

                foreach (var employee in department.Employees)
                {
                    sb.AppendLine($"{employee.FirstName} {employee.LastName} - {employee.JobTitle}");
                }
            }

            return sb.ToString().TrimEnd();
        }
        
        public static string GetEmployee147(SoftUniContext context)
        {
            var employee147 = context.Employees
                .Include(x => x.EmployeesProjects)
                .ThenInclude(x => x.Project)
                .Where(x => x.EmployeeId == 147)
                .Select(x => new
                {
                    x.FirstName,
                    x.LastName,
                    x.JobTitle,
                    EmployeesProjects = x.EmployeesProjects.OrderBy(x => x.Project.Name).ToList(), // error could be here
                })
                .FirstOrDefault();
                

            var sb = new StringBuilder();

            sb.AppendLine($"{employee147.FirstName} {employee147.LastName} - {employee147.JobTitle}");
            foreach (var employeesProjects in employee147.EmployeesProjects)
            {
                sb.AppendLine(employeesProjects.Project.Name);
            }

            return sb.ToString().TrimEnd();
        }

        public static string GetAddressesByTown(SoftUniContext context)
        {
            var addresses = context.Addresses
                .Select(x => new
                {
                    x.AddressText,
                    TownName = x.Town.Name,
                    EmpCount = x.Employees.Count,
                })
                .OrderByDescending(x => x.EmpCount)
                .ThenBy(x => x.TownName)
                .ThenBy(x => x.AddressText)
                .Take(10)
                .ToList();

            var sb = new StringBuilder();

            foreach (var address in addresses)
            {
                sb.AppendLine($"{address.AddressText}, {address.TownName} - {address.EmpCount} employees");
            }

            return sb.ToString().TrimEnd();
        }

        public static string GetEmployeesInPeriod(SoftUniContext context)
        {
            var employees = context.Employees
                .Include(x => x.EmployeesProjects)
                .ThenInclude(x => x.Project)
                .Where(x => x.EmployeesProjects.Any(p => p.Project.StartDate.Year >= 2001 && p.Project.StartDate.Year <= 2003))
                .Take(10)
                .Select(x => new
                {
                    x.FirstName,
                    x.LastName,
                    ManagerFirstName = x.Manager.FirstName,
                    ManagerLastName = x.Manager.LastName,
                    EmployeeProjects = x.EmployeesProjects
                })
                .ToList();

            var sb = new StringBuilder();

            foreach (var employee in employees)
            {
                sb.AppendLine($"{employee.FirstName} {employee.LastName} - Manager: {employee.ManagerFirstName} {employee.ManagerLastName}");

                foreach (var employeeProject in employee.EmployeeProjects)
                {
                    string projectEndDateString = "not finished";
                    if (employeeProject.Project.EndDate != null)
                    {
                        projectEndDateString = employeeProject.Project.EndDate?.ToString("M/d/yyyy h:mm:ss tt");
                    }

                    sb.AppendLine($"--{employeeProject.Project.Name} - {employeeProject.Project.StartDate.ToString("M/d/yyyy h:mm:ss tt")} - {projectEndDateString}");
                }
            }

            return sb.ToString().TrimEnd();
        }

        public static string AddNewAddressToEmployee(SoftUniContext context)
        {
            var nakov = context.Employees
                .FirstOrDefault(x => x.LastName == "Nakov");

            nakov.Address = new Address()
            {
                AddressText = "Vitoshka 15",
                TownId = 4,
            };

            context.SaveChanges();

            var employees = context.Employees
                .Select(x => new { x.AddressId, x.Address.AddressText })
                .OrderByDescending(x => x.AddressId)
                .Take(10)
                .ToList();
            var sb = new StringBuilder();

            foreach (var employee in employees)
            {
                sb.AppendLine($"{employee.AddressText}");
            }

            return sb.ToString().TrimEnd();
        }

        public static string GetEmployeesFromResearchAndDevelopment(SoftUniContext context)
        {
            var employees = context.Employees
               .Where(x => x.Department.Name == "Research and Development")
               .Select(x => new { x.FirstName, x.LastName, x.Salary, DepName = x.Department.Name })
               .OrderBy(x => x.Salary)
               .ThenByDescending(x => x.FirstName)
               .ToList();

            var sb = new StringBuilder();

            foreach (var employee in employees)
            {
                sb.AppendLine($"{employee.FirstName} {employee.LastName} from {employee.DepName} - ${employee.Salary:F2}");
            }

            return sb.ToString().TrimEnd();
        }

        public static string GetEmployeesWithSalaryOver50000(SoftUniContext context)
        {
            var employees = context.Employees
               .Where(x => x.Salary > 50000)
               .Select(x => new { x.FirstName, x.Salary })
               .OrderBy(x => x.FirstName)
               .ToList();

            var sb = new StringBuilder();

            foreach (var employee in employees)
            {
                sb.AppendLine($"{employee.FirstName} - {employee.Salary:F2}");
            }

            return sb.ToString().TrimEnd();
        }

        public static string GetEmployeesFullInformation(SoftUniContext context)
        {
            var employees = context.Employees
                .Select(x => new { x.EmployeeId, x.FirstName, x.MiddleName, x.LastName, x.JobTitle, x.Salary })
                .OrderBy(x => x.EmployeeId)
                .ToList();

            var sb = new StringBuilder();

            foreach (var employee in employees)
            {
                sb.AppendLine($"{employee.FirstName} {employee.LastName} {employee.MiddleName} {employee.JobTitle} {employee.Salary:F2}");
            }

            return sb.ToString().TrimEnd();
        }
    }
}
