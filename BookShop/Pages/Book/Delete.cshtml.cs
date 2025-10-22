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

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return RedirectToPage("./Index");
            }

            var book = await _bookRepository.GetBookByIdAsync(id.Value);
            if (book == null)
            {
                return RedirectToPage("./Index");
            }

            Book = book;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            await _bookRepository.DeleteBookAsync(Book.BookId);
            TempData["SuccessMessage"] = "Book deleted successfully!";
            return RedirectToPage("./Index");
        }
    }
}
