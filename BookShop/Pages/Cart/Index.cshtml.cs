using BookShop.Models;
using BookShop.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace BookShop.Pages.Cart
{
    public class IndexModel : PageModel
    {
        private readonly ICartRepository _cartRepository;

        public IndexModel(ICartRepository cartRepository)
        {
            _cartRepository = cartRepository;
        }

        public List<CartItemDto> CartItems { get; set; } = new();
        public decimal TotalAmount { get; set; }
        public string SearchTerm { get; set; } = string.Empty;

        public async Task OnGetAsync(string searchTerm)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "guest";
            SearchTerm = searchTerm ?? string.Empty;

            IEnumerable<CartItemDto> items;
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                items = await _cartRepository.GetCartItemsAsync(userId);
            }
            else
            {
                items = await _cartRepository.SearchCartItemsAsync(userId, searchTerm);
            }

            CartItems = items.ToList();
            TotalAmount = CartItems.Sum(item => item.Total);
        }

        public async Task<IActionResult> OnPostRemoveAsync(int bookId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "guest";
            await _cartRepository.RemoveFromCartAsync(userId, bookId);
            TempData["SuccessMessage"] = "Item removed from cart!";
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostClearAsync()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "guest";
            await _cartRepository.ClearCartAsync(userId);
            TempData["SuccessMessage"] = "Cart cleared!";
            return RedirectToPage();
        }
    }
}
