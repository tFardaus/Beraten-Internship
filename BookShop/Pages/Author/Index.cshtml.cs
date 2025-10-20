using BookShop.Models;
using BookShop.Services;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BookShop.Pages.Author
{
    public class IndexModel : PageModel
    {
        private readonly IAuthorRepository _authorRepository;

        public IndexModel(IAuthorRepository authorRepository)
        {
            _authorRepository = authorRepository;
        }

        public List<Models.Author> Authors { get; set; } = new List<Models.Author>();

        public void OnGet()
        {
            Authors = _authorRepository.GetAllAuthors().ToList();
        }
    }
}
