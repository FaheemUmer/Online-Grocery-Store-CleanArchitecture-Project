using System.Diagnostics;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Web.Models;

namespace Web.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IHomeService _homeService;

    public HomeController(ILogger<HomeController> logger, IHomeService homeService)
    {
        _logger = logger;
        _homeService = homeService;
    }

    // Home page view
    public IActionResult Index()
    {
        return View();
    }

    // Privacy page view
    public IActionResult Privacy()
    {
        return View();
    }

    // Error handling view
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    // About page view
    public IActionResult About()
    {
        return View();
    }

    // Contact page view
    public IActionResult Contact()
    {
        return View();
    }

    // GET: Profile
    public async Task<IActionResult> Profile()
    {
        var username = HttpContext.Session.GetString("Username");

        if (string.IsNullOrEmpty(username))
        {
            return RedirectToAction("Login", "Account");
        }

        var user = await _homeService.GetUserByUsernameAsync(username);

        if (user == null)
        {
            return RedirectToAction("Login", "Account");
        }

        return View(user);
    }

    public async Task<IActionResult> EditProfile()
    {
        var userId = HttpContext.Session.GetString("UserId");

        if (userId == null)
        {
            return RedirectToAction("Login", "Account");
        }

        var user = await _homeService.GetUserByIdAsync(userId);

        if (user == null)
        {
            return NotFound();
        }

        return View(user);
    }

    [HttpPost]
    public async Task<IActionResult> EditProfile(User model)
    {
        var userId = HttpContext.Session.GetString("UserId");

        if (userId == null)
        {
            return RedirectToAction("Login", "Account");
        }

        var user = await _homeService.GetUserByIdAsync(userId);

        if (user == null)
        {
            return NotFound();
        }

        user.Username = model.Username;
        user.Email = model.Email;
        user.UserType = model.UserType;

        await _homeService.UpdateUserAsync(user);

        return RedirectToAction("Profile", "Home");
    }

    public IActionResult Logout()
    {
        HttpContext.Session.Clear();
        return RedirectToAction("Login", "Account");
    }

    [HttpGet]
    public async Task<IActionResult> Search(string query)
    {
        var searchResults = await _homeService.SearchProductsAsync(query);
        return View(searchResults);
    }
}
