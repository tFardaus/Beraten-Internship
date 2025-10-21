using BookShop.Models;

namespace BookShop.Services
{
    public interface IPublisherRepository
    {
        IEnumerable<Publisher> GetAllPublishers();
        IEnumerable<Publisher> SearchPublishers(string searchTerm);
        Publisher? GetPublisherById(int id);
        void AddPublisher(Publisher publisher);
        void UpdatePublisher(Publisher publisher);
        void DeletePublisher(int id);
    }
}
