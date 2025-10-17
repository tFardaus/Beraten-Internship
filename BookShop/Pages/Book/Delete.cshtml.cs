using BookShop.Models;
using BookShop.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BookShop.Pages.Book
{
    public class DeleteModel : PageModel
    {
        private readonly IBookRepository _bookRepository;

        public DeleteModel(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }

        [BindProperty]
        public Models.Book Book { get; set; } = new Models.Book();

        public IActionResult OnGet(int? id)
        {
            if (id == null)
            {
                return RedirectToPage("./Index");
            }

            var book = _bookRepository.GetBookById(id.Value);
            if (book == null)
            {
                return RedirectToPage("./Index");
            }

            Book = book;
            return Page();
        }

        public IActionResult OnPost()
        {
            _bookRepository.DeleteBook(Book.BookId);
            return RedirectToPage("./Index");
        }
    }
}
