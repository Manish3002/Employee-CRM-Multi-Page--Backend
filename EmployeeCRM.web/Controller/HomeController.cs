using Microsoft.AspNetCore.Mvc;
using EmployeeCRM.Web.Services;
using EmployeeCRM.Web.Models;

namespace EmployeeCRM.Web.Controllers;

public class HomeController : Controller
{
    private readonly IEmployeeApiService _employeeApiService;

    public HomeController(IEmployeeApiService employeeApiService)
    {
        _employeeApiService = employeeApiService;
    }

    // GET: Home/Index
    public async Task<IActionResult> Index()
    {
        var employees = await _employeeApiService.GetEmployeesAsync();
        return View(employees);
    }

    // GET: Home/Details/5
    public async Task<IActionResult> Details(int id)
    {
        var employee = await _employeeApiService.GetEmployeeAsync(id);
        if (employee == null)
        {
            return NotFound();
        }
        return View(employee);
    }

    // GET: Home/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: Home/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(EmployeeViewModel employee)
    {
        if (ModelState.IsValid)
        {
            await _employeeApiService.CreateEmployeeAsync(employee);
            return RedirectToAction(nameof(Index));
        }
        return View(employee);
    }

    // GET: Home/Edit/5
    public async Task<IActionResult> Edit(int id)
    {
        var employee = await _employeeApiService.GetEmployeeAsync(id);
        if (employee == null)
        {
            return NotFound();
        }
        return View(employee);
    }

    // POST: Home/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, EmployeeViewModel employee)
    {
        if (id != employee.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            await _employeeApiService.UpdateEmployeeAsync(id, employee);
            return RedirectToAction(nameof(Index));
        }
        return View(employee);
    }

    // GET: Home/Delete/5
    public async Task<IActionResult> Delete(int id)
    {
        var employee = await _employeeApiService.GetEmployeeAsync(id);
        if (employee == null)
        {
            return NotFound();
        }
        return View(employee);
    }

    // POST: Home/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        await _employeeApiService.DeleteEmployeeAsync(id);
        return RedirectToAction(nameof(Index));
    }
}