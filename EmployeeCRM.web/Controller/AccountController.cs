using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace EmployeeCRM.Web.Controllers;

public class AccountController : Controller
{
    private readonly HttpClient _httpClient;

    public AccountController(HttpClient httpClient)
    {
        _httpClient = httpClient;
        _httpClient.BaseAddress = new Uri("https://localhost:7001/");
    }

    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(string email, string password)
    {
        var response = await _httpClient.PostAsync($"api/auth/login?email={email}&password={password}", null);

        if (response.IsSuccessStatusCode)
        {
            var json = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<LoginResponse>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            HttpContext.Session.SetString("UserEmail", email);
            HttpContext.Session.SetString("UserRole", result?.Role ?? "Employee");

            return RedirectToAction("Index", "Dashboard");
        }

        ViewBag.Error = "Invalid email or password";
        return View();
    }

    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Register(string email, string password, string role)
    {
        var response = await _httpClient.PostAsync($"api/auth/register?email={email}&password={password}&role={role}", null);

        if (response.IsSuccessStatusCode)
        {
            return RedirectToAction("Login");
        }

        ViewBag.Error = "Registration failed. Email may already exist.";
        return View();
    }

    public IActionResult Logout()
    {
        HttpContext.Session.Clear();
        return RedirectToAction("Login");
    }
}

public class LoginResponse
{
    public bool Success { get; set; }
    public string? Message { get; set; }
    public string? Email { get; set; }
    public string? Role { get; set; }
}