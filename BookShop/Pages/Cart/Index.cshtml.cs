using BookShop.Models;
using BookShop.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BookShop.Pages.Cart
{
    public class IndexModel : PageModel
    {
        public List<CartItem> CartItems { get; set; } = new();
        public decimal TotalAmount { get; set; }

        /// <summary>
        /// Load cart items from Session
        /// </summary>
        public void OnGet()
        {
            // Retrieve cart from session
            CartItems = HttpContext.Session.GetObject<List<CartItem>>("Cart") ?? new List<CartItem>();
            
            // Calculate total
            TotalAmount = CartItems.Sum(item => item.Total);
        }

        /// <summary>
        /// Remove item from cart
        /// </summary>
        public IActionResult OnPostRemove(int bookId)
        {
            var cart = HttpContext.Session.GetObject<List<CartItem>>("Cart") ?? new List<CartItem>();
            
            // Remove item
            cart.RemoveAll(c => c.BookId == bookId);
            
            // Save back to session
            HttpContext.Session.SetObject("Cart", cart);
            
            TempData["SuccessMessage"] = "Item removed from cart!";
            return RedirectToPage();
        }

        /// <summary>
        /// Clear entire cart
        /// </summary>
        public IActionResult OnPostClear()
        {
            // Remove cart from session
            HttpContext.Session.Remove("Cart");
            
            TempData["SuccessMessage"] = "Cart cleared!";
            return RedirectToPage();
        }
    }
}
