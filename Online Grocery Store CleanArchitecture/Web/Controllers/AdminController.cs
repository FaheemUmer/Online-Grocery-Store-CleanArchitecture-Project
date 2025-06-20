using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Web.Controllers
{
    public class AdminController : Controller
    {
        private readonly IAdminService _adminService;
        private readonly MyApplicationContext _context;
        private readonly ILogger<AdminController> _logger;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public AdminController(IWebHostEnvironment webHostEnvironment,MyApplicationContext context,IAdminService adminService, ILogger<AdminController> logger)
        {
            _adminService = adminService;
            _logger = logger;
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        // READ: List all products with pagination and search functionality
        public async Task<IActionResult> ManageProduct(string searchQuery = "", int page = 1)
        {
            int pageSize = 10;
            var productsQuery = await _adminService.GetProductsAsync(searchQuery);

            var totalProducts = await productsQuery.CountAsync();
            var totalPages = (int)Math.Ceiling((double)totalProducts / pageSize);

            var products = productsQuery
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            ViewData["CurrentPage"] = page;
            ViewData["TotalPages"] = totalPages;
            ViewData["SearchQuery"] = searchQuery;

            return View(products);
        }

        public async Task<IActionResult> Dashboard()
        {
            var stats = await _adminService.GetDashboardStatsAsync();
            return View(stats); // Pass DashboardViewModel to the view
        }

        public IActionResult Index()
        {
            return View();
        }

        // CREATE: Display Add Product form
        [HttpGet]
        public IActionResult AddProduct()
        {
            var categories = _context.Categories.ToList();
            ViewBag.Categories = new SelectList(categories, "Id", "Name"); // Id = CategoryId, Name = display text

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddProduct(Product model, IFormFile ProductImage)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Categories = new SelectList(_context.Categories, "Id", "Name");
                return View(model);
            }

            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Please fill in all required fields correctly.";
                ViewBag.Categories = _context.Categories.ToList(); // Refill the dropdown
                return View(model);
            }

            // Upload logic
            if (ProductImage != null && ProductImage.Length > 0)
            {
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(ProductImage.FileName);
                var filePath = Path.Combine(_webHostEnvironment.WebRootPath, "images", fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await ProductImage.CopyToAsync(stream);
                }

                model.ImageUrl = "/images/" + fileName;
            }

            // IMPORTANT: Do NOT assign model.Category = null (or anything). Just use CategoryId.
            //_context.Products.Add(model);
            //await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Product added successfully!";
            return RedirectToAction("AddProduct");
        }


        // UPDATE: Display Edit Product form
        public async Task<IActionResult> EditProduct(int id)
        {
            var product = await _adminService.GetProductByIdAsync(id);
            if (product == null)
            {
                TempData["ErrorMessage"] = "Product not found!";
                return RedirectToAction("Index");
            }
            return View(product);
        }

        // UPDATE: Handle Edit Product form submission
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditProduct(Product product)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    //await _adminService.UpdateProductAsync(product);
                    TempData["SuccessMessage"] = "Product updated successfully!";
                    _logger.LogInformation("Product updated: {ProductName}", product.Name);
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    TempData["ErrorMessage"] = ex.Message;
                }
            }
            return View(product);
        }

        // DELETE: Display Delete Confirmation
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = await _adminService.GetProductByIdAsync(id);
            if (product == null)
            {
                TempData["ErrorMessage"] = "Product not found!";
                return RedirectToAction("Index");
            }
            return View(product);
        }

        // DELETE: Handle Delete Confirmation
        [HttpPost, ActionName("DeleteProduct")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await _adminService.DeleteProductAsync(id);
                TempData["SuccessMessage"] = "Product deleted successfully!";
                _logger.LogInformation("Product deleted: {ProductId}", id);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
            }
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Profile()
        {
            var username = HttpContext.Session.GetString("Username");
            if (string.IsNullOrEmpty(username))
            {
                return RedirectToAction("Login", "Account");
            }

            var user = await _adminService.GetUserByUsernameAsync(username);
            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }

            return View(user);
        }

        public async Task<IActionResult> Orders()
        {
            var orders = await _adminService.GetOrdersAsync();
            return View(orders);
        }
    }
}
