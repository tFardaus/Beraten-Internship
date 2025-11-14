using BookShop.Models;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace BookShop.Services
{
    public class SqlCartRepository : ICartRepository
    {
        private readonly AppDbContext _context;

        public SqlCartRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Cart?> GetCartByUserIdAsync(string userId)
        {
            try
            {
                return await _context.Carts
                    .AsNoTracking()
                    .FirstOrDefaultAsync(c => c.UserId == userId);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<CartItemDto>> GetCartItemsAsync(string userId)
        {
            try
            {
                var cart = await GetCartByUserIdAsync(userId);
                if (cart == null || string.IsNullOrEmpty(cart.CartItemsJson))
                    return new List<CartItemDto>();

                return JsonSerializer.Deserialize<List<CartItemDto>>(cart.CartItemsJson) ?? new List<CartItemDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task AddToCartAsync(string userId, int bookId, int quantity = 1)
        {
            try
            {
                var cart = await _context.Carts.FirstOrDefaultAsync(c => c.UserId == userId);

                if (cart == null)
                {
                    cart = new Cart { UserId = userId };
                    _context.Carts.Add(cart);
                }

                var items = string.IsNullOrEmpty(cart.CartItemsJson)
                    ? new List<CartItemDto>()
                    : JsonSerializer.Deserialize<List<CartItemDto>>(cart.CartItemsJson) ?? new List<CartItemDto>();

                var existingItem = items.FirstOrDefault(i => i.BookId == bookId);
                
                if (existingItem != null)
                {
                    existingItem.Quantity += quantity;
                }
                else
                {
                    var book = await _context.Books.FindAsync(bookId);
                    if (book != null)
                    {
                        items.Add(new CartItemDto
                        {
                            BookId = bookId,
                            BookTitle = book.BookTitle,
                            Price = book.Price,
                            Quantity = quantity
                        });
                    }
                }

                cart.CartItemsJson = JsonSerializer.Serialize(items);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task RemoveFromCartAsync(string userId, int bookId)
        {
            try
            {
                var cart = await _context.Carts.FirstOrDefaultAsync(c => c.UserId == userId);
                
                if (cart == null || string.IsNullOrEmpty(cart.CartItemsJson)) return;

                var items = JsonSerializer.Deserialize<List<CartItemDto>>(cart.CartItemsJson) ?? new List<CartItemDto>();
                items.RemoveAll(i => i.BookId == bookId);
                
                cart.CartItemsJson = JsonSerializer.Serialize(items);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task ClearCartAsync(string userId)
        {
            try
            {
                var cart = await _context.Carts.FirstOrDefaultAsync(c => c.UserId == userId);
                
                if (cart == null) return;

                cart.CartItemsJson = JsonSerializer.Serialize(new List<CartItemDto>());
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<CartItemDto>> SearchCartItemsAsync(string userId, string searchTerm)
        {
            try
            {
                var cart = await _context.Carts.FirstOrDefaultAsync(c => c.UserId == userId);

                if (cart == null || string.IsNullOrEmpty(cart.CartItemsJson))
                    return new List<CartItemDto>();

                var items = JsonSerializer.Deserialize<List<CartItemDto>>(cart.CartItemsJson) ?? new List<CartItemDto>();
                return items.Where(i => i.BookTitle.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
