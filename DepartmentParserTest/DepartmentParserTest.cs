using DepartmentParser.Controllers;
using DepartmentParser.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace DepartmentParserTest;

[TestClass]
public class DepartmentParserTest
{
    [TestMethod]
    public void TestRootElements()
    {
        var controller = new DepartmentController();
        var result = controller.TestGet("../../../../DepartmentParser/example.csv");
        var departments = JsonConvert.DeserializeObject<List<Department>>(result);
        Assert.AreEqual(2, departments!.Count);
    }

    [TestMethod]
    public void TestDescendentCount()
    {
        var controller = new DepartmentController();
        var result = controller.TestGet("../../../../DepartmentParser/example.csv");
        var departments = JsonConvert.DeserializeObject<List<Department>>(result);
        Assert.AreEqual(4, departments![0].Departments.Count);
        Assert.AreEqual(4, departments![1].Departments.Count);
    }

    [TestMethod]
    public void TestNumDescendants()
    {
        var controller = new DepartmentController();
        var result = controller.TestGet("../../../../DepartmentParser/example.csv");
        var departments = JsonConvert.DeserializeObject<List<Department>>(result);
        Assert.AreEqual(4, departments![0].NumDescendants);
        Assert.AreEqual(4, departments![1].NumDescendants);
    }

    [TestMethod]
    public void TestRootElementsNested()
    {
        var controller = new DepartmentController();
        var result = controller.TestGet("../../../../DepartmentParser/nested.csv");
        var departments = JsonConvert.DeserializeObject<List<Department>>(result);
        Assert.AreEqual(2, departments!.Count);
    }

    [TestMethod]
    public void TestDescendentCountNested()
    {
        var controller = new DepartmentController();
        var result = controller.TestGet("../../../../DepartmentParser/nested.csv");
        var departments = JsonConvert.DeserializeObject<List<Department>>(result);
        Assert.AreEqual(4, departments![0].Departments.Count);
        Assert.AreEqual(4, departments![1].Departments.Count);
    }

    [TestMethod]
    public void TestNumDescendantsNested()
    {
        var controller = new DepartmentController();
        var result = controller.TestGet("../../../../DepartmentParser/nested.csv");
        var departments = JsonConvert.DeserializeObject<List<Department>>(result);
        Assert.AreEqual(4, departments![0].NumDescendants);
        Assert.AreEqual(8, departments![1].NumDescendants);
    }

    [TestMethod]
    public void TestDescendantsForChildrenNested()
    {
        var controller = new DepartmentController();
        var result = controller.TestGet("../../../../DepartmentParser/nested.csv");
        var departments = JsonConvert.DeserializeObject<List<Department>>(result);
        Assert.AreEqual(4, departments![1].Departments[3].NumDescendants);
        Assert.AreEqual(2, departments![1].Departments[3].Departments.Count);
    }

    [TestMethod]
    public void TestManyNestedCount()
    {
        var controller = new DepartmentController();
        var result = controller.TestGet("../../../../DepartmentParser/manyNested.csv");
        var departments = JsonConvert.DeserializeObject<List<Department>>(result);
        Assert.AreEqual(5, departments![0].NumDescendants);
        Assert.AreEqual(1, departments![0].Departments.Count);
    }
}
