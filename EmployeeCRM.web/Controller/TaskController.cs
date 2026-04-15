using Microsoft.AspNetCore.Mvc;
using EmployeeCRM.Web.Services;
using EmployeeCRM.Web.Models;

namespace EmployeeCRM.Web.Controllers;

public class TaskController : Controller
{
    private readonly ITaskApiService _taskService;
    private readonly IEmployeeApiService _employeeService;
    private readonly IClientApiService _clientService;

    public TaskController(ITaskApiService taskService, IEmployeeApiService employeeService, IClientApiService clientService)
    {
        _taskService = taskService;
        _employeeService = employeeService;
        _clientService = clientService;
    }

    public async Task<IActionResult> Index()
    {
        var tasks = await _taskService.GetTasksAsync();
        return View(tasks);
    }

    public async Task<IActionResult> Details(int id)
    {
        var task = await _taskService.GetTaskAsync(id);
        if (task == null) return NotFound();
        return View(task);
    }

    public async Task<IActionResult> Create()
    {
        ViewBag.Employees = await _employeeService.GetEmployeesAsync();
        ViewBag.Clients = await _clientService.GetClientsAsync();
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(TaskViewModel task)
    {
        await _taskService.CreateTaskAsync(task);
        return RedirectToAction("Index");
    }

    public async Task<IActionResult> Edit(int id)
    {
        var task = await _taskService.GetTaskAsync(id);
        if (task == null) return NotFound();
        ViewBag.Employees = await _employeeService.GetEmployeesAsync();
        ViewBag.Clients = await _clientService.GetClientsAsync();
        return View(task);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(TaskViewModel task)
    {
        await _taskService.UpdateTaskAsync(task.Id, task);
        return RedirectToAction("Index");
    }

    public async Task<IActionResult> Delete(int id)
    {
        var task = await _taskService.GetTaskAsync(id);
        if (task == null) return NotFound();
        return View(task);
    }

    [HttpPost, ActionName("Delete")]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        await _taskService.DeleteTaskAsync(id);
        return RedirectToAction("Index");
    }

    [HttpPost]
    public async Task<IActionResult> UpdateStatus(int id, string status)
    {
        await _taskService.UpdateTaskStatusAsync(id, status);
        return RedirectToAction("Index");
    }

    // Filter by Status - This is the missing method
    public async Task<IActionResult> ByStatus(string status)
    {
        var tasks = await _taskService.GetTasksByStatusAsync(status);
        ViewBag.Status = status;
        return View(tasks);
    }

    // Convenience methods
    public async Task<IActionResult> Pending()
    {
        return await ByStatus("Pending");
    }

    public async Task<IActionResult> InProgress()
    {
        return await ByStatus("InProgress");
    }

    public async Task<IActionResult> Completed()
    {
        return await ByStatus("Completed");
    }
}