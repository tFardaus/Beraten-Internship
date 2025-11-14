using BookShop.Models;
using BookShop.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace BookShop.Pages.Book
{
    public class IndexModel : PageModel
    {
        private readonly IBookRepository bookRepository;
        private readonly ICartRepository cartRepository;

        public IndexModel(IBookRepository bookRepository, ICartRepository cartRepository)
        {
            this.bookRepository = bookRepository;
            this.cartRepository = cartRepository;
        }

        public List<Models.Book> Books { get; set; } = new List<Models.Book>();
        public string SearchTerm { get; set; } = string.Empty;
        public string SortOrder { get; set; } = string.Empty;

        public async Task OnGetAsync(string searchTerm, string sortOrder)
        {
            SearchTerm = searchTerm ?? string.Empty;
            SortOrder = sortOrder ?? string.Empty;
            
            try
            {
                IEnumerable<Models.Book> books;
                
                if (string.IsNullOrWhiteSpace(searchTerm))
                {
                    books = await this.bookRepository.GetAllBookAsync();
                }
                else
                {
                    books = await this.bookRepository.SearchBooksAsync(searchTerm);
                }
                
               
                Books = sortOrder switch
                {
                    "price_asc" => books.OrderBy(b => b.Price).ToList(),
                    "price_desc" => books.OrderByDescending(b => b.Price).ToList(),
                    _ => books.ToList()
                };
            }
            catch (OperationCanceledException)
            {
                Console.WriteLine("Cancellation token used to kill the task");
            }
        }

        
        public async Task<IActionResult> OnPostAddToCartAsync(int id)
        {
            var book = await this.bookRepository.GetBookByIdAsync(id);
            if (book == null)
            {
                TempData["ErrorMessage"] = "Book not found!";
                return RedirectToPage();
            }

            var userId = User.FindFirstValue(System.Security.Claims.ClaimTypes.NameIdentifier) ?? "guest";
            await this.cartRepository.AddToCartAsync(userId, id, 1);
            TempData["SuccessMessage"] = $"{book.BookTitle} added to cart!";
            return RedirectToPage();
        }
    }
}
