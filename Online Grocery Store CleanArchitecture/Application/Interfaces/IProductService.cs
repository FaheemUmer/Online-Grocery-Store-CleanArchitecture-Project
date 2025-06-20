using Domain.Entities;

namespace Application.Interfaces
{
    public interface IProductService
    {
        List<Product> GetAllProducts();
        List<Product> GetProductsByCategory(string category);
        List<CartItem> GetCart(string cartJson);
        string SaveCart(List<CartItem> cart);
        void AddToCart(int id, int quantity, ref List<CartItem> cart);
        void RemoveFromCart(int id, ref List<CartItem> cart);
        decimal GetTotalAmount(List<CartItem> cart);
    }
}
