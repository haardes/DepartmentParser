using DepartmentParser.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace DepartmentParser.Controllers;

[ApiController]
[Route("[controller]")]
public class DepartmentController : ControllerBase
{
    private string _filename = "example.csv";

    [HttpGet]
    public ActionResult<string> Get()
    {
        try
        {
            var departments = ParseCsvToDepartments(_filename);
            var result = CreateDepartmentHierarchy(departments);
            CountDescendants(result);
            var json = JsonConvert.SerializeObject(result);

            return new OkObjectResult(json);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("test")]
    public string TestGet(string path)
    {
        var departments = ParseCsvToDepartments(path);
        var result = CreateDepartmentHierarchy(departments);
        CountDescendants(result);
        var json = JsonConvert.SerializeObject(result);

        return json;
    }

    private int CountDescendants(List<Department> departments)
    {
        // Recursively count children
        foreach (var department in departments)
        {
            department.NumDescendants += department.Departments.Count;
            department.NumDescendants += CountDescendants(department.Departments);
        }

        // Return total number of children
        return departments.Sum(d => d.NumDescendants);
    }

    private static List<Department> CreateDepartmentHierarchy(IEnumerable<Department> departments)
    {
        List<Department> result = new();

        // Find children of each department
        foreach (var department in departments)
        {
            if (department.DepartmentParent_OID == 0)
            {
                continue; // Skip root departments
            }

            var parent = departments.FirstOrDefault(d => d.OID == department.DepartmentParent_OID);
            if (parent != null)
            {
                parent.Departments.Add(department);
            }
        }

        // Add root departments to result
        foreach (var department in departments)
        {
            if (department.DepartmentParent_OID == 0)
            {
                result.Add(department);
            }
        }

        return result;
    }

    private static List<Department> ParseCsvToDepartments(string path)
    {
        var departments = new List<Department>();
        var csv = System.IO.File.ReadAllText(path);
        var lines = csv.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
        foreach (var line in lines)
        {
            var values = line.Split(',');
            if (values.Length != 4) throw new ArgumentException("CSV file is not in the correct format. Please check the CSV file.");

            // If OID can't be parsed and departments is empty, it is probably the headers, else throw exception
            if (departments.Count == 0 && !int.TryParse(values[0], out int OID)) continue;
            else if (!int.TryParse(values[0], out OID)) throw new ArgumentException("Department ID could not be parsed as integer. Please check the CSV file.");

            _ = int.TryParse(values[3], out int DepartmentParent_OID);

            var department = new Department(OID, values[1], values[2], DepartmentParent_OID);
            departments.Add(department);
        }

        return departments;
    }
}