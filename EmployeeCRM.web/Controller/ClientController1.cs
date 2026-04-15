using Microsoft.AspNetCore.Mvc;
using EmployeeCRM.Web.Services;
using EmployeeCRM.Web.Models;

namespace EmployeeCRM.Web.Controllers;

public class ClientController : Controller
{
    private readonly IClientApiService _clientService;
    private readonly IEmployeeApiService _employeeService;

    public ClientController(IClientApiService clientService, IEmployeeApiService employeeService)
    {
        _clientService = clientService;
        _employeeService = employeeService;
    }

    public async Task<IActionResult> Index()
    {
        var clients = await _clientService.GetClientsAsync();
        return View(clients);
    }

    public async Task<IActionResult> Details(int id)
    {
        var client = await _clientService.GetClientAsync(id);
        if (client == null) return NotFound();
        return View(client);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(ClientViewModel client)
    {
        await _clientService.CreateClientAsync(client);
        return RedirectToAction("Index");
    }

    public async Task<IActionResult> Edit(int id)
    {
        var client = await _clientService.GetClientAsync(id);
        if (client == null) return NotFound();
        return View(client);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(ClientViewModel client)
    {
        await _clientService.UpdateClientAsync(client.Id, client);
        return RedirectToAction("Index");
    }

    public async Task<IActionResult> Delete(int id)
    {
        var client = await _clientService.GetClientAsync(id);
        if (client == null) return NotFound();
        return View(client);
    }

    [HttpPost, ActionName("Delete")]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        await _clientService.DeleteClientAsync(id);
        return RedirectToAction("Index");
    }

    public async Task<IActionResult> Assign(int id)
    {
        var client = await _clientService.GetClientAsync(id);
        if (client == null) return NotFound();

        var employees = await _employeeService.GetEmployeesAsync();
        ViewBag.Employees = employees;
        return View(client);
    }

    [HttpPost]
    public async Task<IActionResult> Assign(int clientId, int employeeId)
    {
        await _clientService.AssignClientToEmployeeAsync(clientId, employeeId);
        return RedirectToAction("Index");
    }

    public async Task<IActionResult> ByEmployee(int id)
    {
        var clients = await _clientService.GetClientsByEmployeeAsync(id);
        var employee = await _employeeService.GetEmployeeAsync(id);
        ViewBag.EmployeeName = employee?.FullName ?? "Employee";
        return View(clients);
    }
    public IActionResult Test()
    {
        return Content("Client Controller is working! ID: " + DateTime.Now);
    }


    public async Task<IActionResult> Unassigned()
    {
        var clients = await _clientService.GetUnassignedClientsAsync();
        return View(clients);
    }
}