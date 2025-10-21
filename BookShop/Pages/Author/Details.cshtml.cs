using BookShop.Models;
using BookShop.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BookShop.Pages.Author
{
    public class DetailsModel : PageModel
    {
        private readonly IAuthorRepository _authorRepository;

        public DetailsModel(IAuthorRepository authorRepository)
        {
            _authorRepository = authorRepository;
        }

        public AuthorWithBooksDto? AuthorInfo { get; set; }
        public List<AuthorWithBooksDto> Books { get; set; } = new();

        public IActionResult OnGet(int id)
        {
            var results = _authorRepository.GetAuthorWithBooks(id).ToList();

            if (!results.Any())
            {
                return NotFound();
            }

            AuthorInfo = results.First();
            Books = results.Where(r => r.BookId.HasValue).ToList();

            return Page();
        }
    }
}
