using BookShop.Models;
using BookShop.Services;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;

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
        public Dictionary<int, AuthorData> AuthorJsonData { get; set; } = new();

        public async Task OnGetAsync(string searchTerm)
        {
            SearchTerm = searchTerm ?? string.Empty;
            
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                Authors = (await _authorRepository.GetAllAuthorsAsync()).ToList();
            }
            else
            {
                Authors = (await _authorRepository.SearchAuthorsAsync(searchTerm)).ToList();
            }

            foreach (var author in Authors)
            {
                if (!string.IsNullOrEmpty(author.AuthorDataJson))
                {
                    var data = JsonSerializer.Deserialize<AuthorData>(author.AuthorDataJson);
                    if (data != null)
                    {
                        AuthorJsonData[author.AuthorId] = data;
                    }
                }
            }
        }
    }

    public class AuthorData
    {
        public string Name { get; set; } = string.Empty;
        public string Biography { get; set; } = string.Empty;
    }
}
