using BookShop.Models;
using BookShop.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BookShop.Pages.Author
{
    public class DeleteModel : PageModel
    {
        private readonly IAuthorRepository _authorRepository;

        public DeleteModel(IAuthorRepository authorRepository)
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
            _authorRepository.DeleteAuthor(Author.AuthorId);
            return RedirectToPage("./Index");
        }
    }
}
