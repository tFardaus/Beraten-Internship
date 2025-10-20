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

        public void OnGet()
        {
            Publishers = _publisherRepository.GetAllPublishers().ToList();
        }
    }
}
