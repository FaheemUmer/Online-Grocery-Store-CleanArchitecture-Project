using Domain.Entities;
using Application.Interfaces;
using Infrastructure.Data;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Application.Services
{
    public class ProductService : IProductService
    {
        private readonly MyApplicationContext _context;

        public ProductService(MyApplicationContext context)
        {
            _context = context;
        }

        public List<Product> GetAllProducts()
        {
            return _context.Products.ToList();
        }

        public List<Product> GetProductsByCategory(string category)
        {
            return _context.Products.Where(p => p.Category == category).ToList();
        }

        public List<CartItem> GetCart(string cartJson)
        {
            return string.IsNullOrEmpty(cartJson)
                ? new List<CartItem>()
                : JsonSerializer.Deserialize<List<CartItem>>(cartJson);
        }

        public string SaveCart(List<CartItem> cart)
        {
            return JsonSerializer.Serialize(cart);
        }

        public void AddToCart(int id, int quantity, ref List<CartItem> cart)
        {
            var product = _context.Products.FirstOrDefault(p => p.Id == id);
            if (product == null) return;

            var cartItem = cart.FirstOrDefault(c => c.ProductId == id);
            if (cartItem == null)
            {
                cart.Add(new CartItem { ProductId = product.Id, Quantity = quantity });
            }
            else
            {
                cartItem.Quantity += quantity;
            }
        }

        public void RemoveFromCart(int id, ref List<CartItem> cart)
        {
            var cartItem = cart.FirstOrDefault(c => c.ProductId == id);
            if (cartItem != null)
            {
                cart.Remove(cartItem);
            }
        }

        public decimal GetTotalAmount(List<CartItem> cart)
        {
            return cart.Sum(item => item.Quantity * 100);
        }
    }
}
