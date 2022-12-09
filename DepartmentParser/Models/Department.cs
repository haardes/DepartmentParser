using Newtonsoft.Json;

namespace DepartmentParser.Models;

public class Department
{
    [JsonProperty(Order = 1)]
    public int OID { get; set; }

    [JsonProperty(Order = 2)]
    public string Title { get; set; }

    [JsonProperty(Order = 4)]
    public string Color { get; set; }

    [JsonIgnore]
    public int DepartmentParent_OID { get; set; }

    [JsonProperty(Order = 3)]
    public int NumDescendants = 0;

    [JsonProperty(Order = 5)]
    public List<Department> Departments = new();

    public Department(int oID, string title, string color, int departmentParent_OID)
    {
        OID = oID;
        Title = title;
        Color = color;
        DepartmentParent_OID = departmentParent_OID;
    }
}