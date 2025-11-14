using BookShop.Services;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BookShop.Services.Extensions;

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
        public Dictionary<int, PublisherData> PublisherXmlData { get; set; } = new();

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

            foreach (var publisher in Publishers)
            {
                if (!string.IsNullOrEmpty(publisher.PublisherDataXml))
                {
                    var (name, address, phone) = publisher.PublisherDataXml.ParsePublisherXml();
                    PublisherXmlData[publisher.PublisherId] = new PublisherData
                    {
                        Name = name,
                        Address = address,
                        Phone = phone
                    };
                }
            }
        }
    }

    public class PublisherData
    {
        public string Name { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
    }
}
