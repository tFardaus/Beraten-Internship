using BookShop.Models;

namespace BookShop.Services
{
    public interface IPublisherRepository
    {
        Task<IEnumerable<Publisher>> GetAllPublishersAsync();
        Task<IEnumerable<Publisher>> SearchPublishersAsync(string searchTerm);
        Task<Publisher?> GetPublisherByIdAsync(int id);
        Task AddPublisherAsync(Publisher publisher);
        Task UpdatePublisherAsync(Publisher publisher);
        Task DeletePublisherAsync(int id);
    }
}
