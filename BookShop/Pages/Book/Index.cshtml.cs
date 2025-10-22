using BookShop.Models;
using BookShop.Services;
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

        public async Task OnGetAsync(string searchTerm)
        {
            SearchTerm = searchTerm ?? string.Empty;
            
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                Books = (await _bookRepository.GetAllBookAsync()).ToList();
            }
            else
            {
                Books = (await _bookRepository.SearchBooksAsync(searchTerm)).ToList();
            }
        }
    }
}
