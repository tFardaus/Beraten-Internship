using BookShop.Models;
using BookShop.Services;
using BookShop.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BookShop.Pages.Book
{
    public class IndexModel : PageModel
    {
        private readonly IBookRepository _bookRepository;

        public IndexModel(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }

        public List<Models.Book> Books { get; set; } = new List<Models.Book>();
        public string SearchTerm { get; set; } = string.Empty;
        public string SortOrder { get; set; } = string.Empty;

        public async Task OnGetAsync(string searchTerm, string sortOrder)
        {
            SearchTerm = searchTerm ?? string.Empty;
            SortOrder = sortOrder ?? string.Empty;
            
            IEnumerable<Models.Book> books;
            
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                books = await _bookRepository.GetAllBookAsync();
            }
            else
            {
                books = await _bookRepository.SearchBooksAsync(searchTerm);
            }
            
            // Apply sorting
            Books = sortOrder switch
            {
                "price_asc" => books.OrderBy(b => b.Price).ToList(),
                "price_desc" => books.OrderByDescending(b => b.Price).ToList(),
                _ => books.ToList()
            };
        }

        /// <summary>
        /// Handles Add to Cart button click
        /// Stores cart items in Session
        /// </summary>
        public async Task<IActionResult> OnPostAddToCartAsync(int id)
        {
            // Get book details from database
            var book = await _bookRepository.GetBookByIdAsync(id);
            if (book == null)
            {
                TempData["ErrorMessage"] = "Book not found!";
                return RedirectToPage();
            }

            // Get existing cart from session (or create new empty list)
            var cart = HttpContext.Session.GetObject<List<CartItem>>("Cart") ?? new List<CartItem>();

            // Check if book already in cart
            var existingItem = cart.FirstOrDefault(c => c.BookId == id);
            if (existingItem != null)
            {
                // Increase quantity if already in cart
                existingItem.Quantity++;
            }
            else
            {
                // Add new item to cart
                cart.Add(new CartItem
                {
                    BookId = book.BookId,
                    BookTitle = book.BookTitle,
                    Price = book.Price,
                    Quantity = 1
                });
            }

            // Save updated cart back to session
            HttpContext.Session.SetObject("Cart", cart);

            TempData["SuccessMessage"] = $"{book.BookTitle} added to cart!";
            return RedirectToPage();
        }
    }
}
