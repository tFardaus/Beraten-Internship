using BookShop.Models;
using BookShop.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BookShop.Pages.Author
{
    public class EditModel : PageModel
    {
        private readonly IAuthorRepository _authorRepository;

        public EditModel(IAuthorRepository authorRepository)
        {
            _authorRepository = authorRepository;
        }

        [BindProperty]
        public Models.Author Author { get; set; } = new Models.Author();

        public IActionResult OnGet(int? id)
        {
            if (id == null)
            {
                return RedirectToPage("./Index");
            }

            var author = _authorRepository.GetAuthorById(id.Value);
            if (author == null)
            {
                return RedirectToPage("./Index");
            }

            Author = author;
            return Page();
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _authorRepository.UpdateAuthor(Author);
            return RedirectToPage("./Index");
        }
    }
}
