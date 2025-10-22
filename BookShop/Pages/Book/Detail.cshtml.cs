using BookShop.Models;
using BookShop.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BookShop.Pages.Book
{
    public class DetailModel : PageModel
    {
        private readonly IBookRepository _bookRepository;

        public DetailModel(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }

        public BookDetailsDto BookDetails { get; set; } = new BookDetailsDto();

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return RedirectToPage("./Index");
            }

            var bookDetails = await _bookRepository.GetBookDetailsAsync(id.Value);
            if (bookDetails == null)
            {
                return RedirectToPage("./Index");
            }

            BookDetails = bookDetails;
            return Page();
        }
    }
}
