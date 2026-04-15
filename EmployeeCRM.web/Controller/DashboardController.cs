using Microsoft.AspNetCore.Mvc;
using EmployeeCRM.Web.Services;

namespace EmployeeCRM.Web.Controllers;

public class DashboardController : Controller
{
    private readonly IDashboardService _dashboardService;

    public DashboardController(IDashboardService dashboardService)
    {
        _dashboardService = dashboardService;
    }

    public async Task<IActionResult> Index()
    {
        var userRole = HttpContext.Session.GetString("UserRole") ?? "Employee";
        ViewBag.UserRole = userRole;
        var data = await _dashboardService.GetDashboardDataAsync();
        return View(data);
    }
}