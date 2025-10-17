using BookShop.Models;
using BookShop.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BookShop.Pages.Book
{
    public class EditModel : PageModel
    {
        private readonly IBookRepository _bookRepository;

        public EditModel(IBookRepository bookRepository)
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
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _bookRepository.UpdateBook(Book);
            return RedirectToPage("./Index");
        }
    }
}
