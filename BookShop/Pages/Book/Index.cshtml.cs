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

        public void OnGet()
        {
            Books = _bookRepository.GetAllBook().ToList();
        }
    }
}
