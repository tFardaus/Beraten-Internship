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
        public string SearchTerm { get; set; } = string.Empty;

        public void OnGet(string searchTerm)
        {
            SearchTerm = searchTerm ?? string.Empty;
            
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                Authors = _authorRepository.GetAllAuthors().ToList();
            }
            else
            {
                Authors = _authorRepository.SearchAuthors(searchTerm).ToList();
            }
        }
    }
}
