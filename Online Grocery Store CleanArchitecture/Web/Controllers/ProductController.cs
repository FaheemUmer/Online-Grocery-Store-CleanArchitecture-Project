using Microsoft.AspNetCore.Mvc;
using Application.Interfaces;
using Domain.Entities;


namespace Web.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        public IActionResult Index()
        {
            var products = _productService.GetAllProducts();
            return View(products);
        }

        public IActionResult Checkout()
        {
            var cart = GetCart();
            if (cart.Count == 0)
            {
                TempData["Message"] = "Your cart is empty.";
                return RedirectToAction(nameof(Cart));
            }

            ViewData["TotalAmount"] = _productService.GetTotalAmount(cart);
            return View(cart);
        }

        [HttpPost]
        public IActionResult CompleteCheckout(string userName, string userAddress)
        {
            var cart = GetCart();
            if (cart.Count == 0)
            {
                TempData["Message"] = "Your cart is empty.";
                return RedirectToAction(nameof(Cart));
            }

            SaveCart(new List<CartItem>());
            TempData["SuccessMessage"] = "Your order has been placed successfully!";
            return RedirectToAction("OrderSuccess");
        }

        public IActionResult OrderSuccess()
        {
            return View();
        }

        public IActionResult AddToCart(int id, int quantity)
        {
            var cart = GetCart();
            _productService.AddToCart(id, quantity, ref cart);
            SaveCart(cart);

            TempData["Message"] = "Item added to cart.";
            return RedirectToAction("Cart");
        }

        public IActionResult Cart()
        {
            var cart = GetCart();
            return View(cart);
        }

        public IActionResult RemoveFromCart(int id)
        {
            var cart = GetCart();
            _productService.RemoveFromCart(id, ref cart);
            SaveCart(cart);
            return RedirectToAction(nameof(Cart));
        }

        private List<CartItem> GetCart()
        {
            var cartJson = HttpContext.Session.GetString("Cart");
            return _productService.GetCart(cartJson);
        }

        private void SaveCart(List<CartItem> cart)
        {
            var cartJson = _productService.SaveCart(cart);
            HttpContext.Session.SetString("Cart", cartJson);
        }

        public IActionResult FruitsAndVege() => View(_productService.GetProductsByCategory("Fruits And Vegetables"));

        public IActionResult DairyProd() => View(_productService.GetProductsByCategory("Dairy Products"));

        public IActionResult BreadAndBak() => View(_productService.GetProductsByCategory("Bread And Bakery"));

        public IActionResult SnacksAndConfec() => View(_productService.GetProductsByCategory("Snacks And Confectionary"));

        public IActionResult Beverages() => View(_productService.GetProductsByCategory("Beverages"));
    }
}
