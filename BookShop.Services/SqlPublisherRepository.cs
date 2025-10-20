using BookShop.Models;
using Microsoft.EntityFrameworkCore;

namespace BookShop.Services
{
    public class SqlPublisherRepository : IPublisherRepository
    {
        private readonly AppDbContext _context;

        public SqlPublisherRepository(AppDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Publisher> GetAllPublishers()
        {
            return _context.Publishers.Include(p => p.Books).ToList();
        }

        public Publisher? GetPublisherById(int id)
        {
            return _context.Publishers.Include(p => p.Books).FirstOrDefault(p => p.PublisherId == id);
        }

        public void AddPublisher(Publisher publisher)
        {
            _context.Publishers.Add(publisher);
            _context.SaveChanges();
        }

        public void UpdatePublisher(Publisher publisher)
        {
            _context.Publishers.Update(publisher);
            _context.SaveChanges();
        }

        public void DeletePublisher(int id)
        {
            var publisher = _context.Publishers.Find(id);
            if (publisher != null)
            {
                _context.Publishers.Remove(publisher);
                _context.SaveChanges();
            }
        }
    }
}
