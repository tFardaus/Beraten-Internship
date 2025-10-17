using BookShop.Models;
using BookShop.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BookShop.Pages.Book
{
    public class CreateModel : PageModel
    {
        private readonly IBookRepository _bookRepository;

        public CreateModel(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }

        [BindProperty]
        public Models.Book Book { get; set; } = new Models.Book();

        public IActionResult OnGet()
        {
            return Page();
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _bookRepository.AddBook(Book);
            return RedirectToPage("./Index");
        }
    }
}
