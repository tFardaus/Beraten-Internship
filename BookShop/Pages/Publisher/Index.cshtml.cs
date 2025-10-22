using BookShop.Services;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BookShop.Pages.Publisher
{
    public class IndexModel : PageModel
    {
        private readonly IPublisherRepository _publisherRepository;

        public IndexModel(IPublisherRepository publisherRepository)
        {
            _publisherRepository = publisherRepository;
        }

        public List<Models.Publisher> Publishers { get; set; } = new();
        public string SearchTerm { get; set; } = string.Empty;

        public async Task OnGetAsync(string searchTerm)
        {
            SearchTerm = searchTerm ?? string.Empty;
            
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                Publishers = (await _publisherRepository.GetAllPublishersAsync()).ToList();
            }
            else
            {
                Publishers = (await _publisherRepository.SearchPublishersAsync(searchTerm)).ToList();
            }
        }
    }
}
