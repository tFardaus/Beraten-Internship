using BookShop.Models;

namespace BookShop.Services
{
    public interface ICartRepository
    {
        Task<Cart?> GetCartByUserIdAsync(string userId);
        Task<IEnumerable<CartItemDto>> GetCartItemsAsync(string userId);
        Task AddToCartAsync(string userId, int bookId, int quantity = 1);
        Task RemoveFromCartAsync(string userId, int bookId);
        Task ClearCartAsync(string userId);
        Task<IEnumerable<CartItemDto>> SearchCartItemsAsync(string userId, string searchTerm);
    }
}
