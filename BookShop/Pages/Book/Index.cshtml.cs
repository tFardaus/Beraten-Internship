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

        public void OnGet(string searchTerm)
        {
            SearchTerm = searchTerm ?? string.Empty;
            
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                Books = _bookRepository.GetAllBook().ToList();
            }
            else
            {
                Books = _bookRepository.SearchBooks(searchTerm).ToList();
            }
        }
    }
}
