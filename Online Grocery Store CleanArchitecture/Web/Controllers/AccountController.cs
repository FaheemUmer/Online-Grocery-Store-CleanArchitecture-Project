using Application.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        public IActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignUp(string username, string password, string email, string userType)
        {
            var newUser = new User
            {
                Username = username,
                Password = password,
                Email = email,
                UserType = userType
            };

            try
            {
                await _accountService.SignUpAsync(newUser);
                return RedirectToAction("Login");
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
                return View();
            }
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string username, string password)
        {
            try
            {
                var user = await _accountService.LoginAsync(username, password);

                // Save user details in session
                HttpContext.Session.SetString("UserId", user.UserId.ToString());
                HttpContext.Session.SetString("Username", user.Username);
                HttpContext.Session.SetString("UserType", user.UserType);

                // Redirect based on UserType
                if (user.UserType == "Admin")
                {
                    return RedirectToAction("Index", "Admin");
                }
                else if (user.UserType == "Customer")
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ViewBag.ErrorMessage = "Invalid User Type";
                    return View();
                }
            }
            catch (UnauthorizedAccessException ex)
            {
                ViewBag.ErrorMessage = ex.Message;
                return View();
            }
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear(); // Clear session data
            return RedirectToAction("Login", "Account");
        }
    }
}
