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

        public async Task<IEnumerable<Publisher>> GetAllPublishersAsync()
        {
            return await _context.Publishers.AsNoTracking().Include(p => p.Books).ToListAsync();
        }

        public async Task<IEnumerable<Publisher>> SearchPublishersAsync(string searchTerm)
        {
            return await _context.Publishers.AsNoTracking()
                .Include(p => p.Books)
                .Where(p => p.Name.Contains(searchTerm))
                .ToListAsync();
        }

        public async Task<Publisher?> GetPublisherByIdAsync(int id)
        {
            return await _context.Publishers.AsNoTracking().Include(p => p.Books).FirstOrDefaultAsync(p => p.PublisherId == id);
        }

        public async Task AddPublisherAsync(Publisher publisher)
        {
            _context.Publishers.Add(publisher);
            await _context.SaveChangesAsync();
        }

        public async Task UpdatePublisherAsync(Publisher publisher)
        {
            _context.Publishers.Update(publisher);
            await _context.SaveChangesAsync();
        }

        public async Task DeletePublisherAsync(int id)
        {
            var publisher = await _context.Publishers.FindAsync(id);
            if (publisher != null)
            {
                _context.Publishers.Remove(publisher);
                await _context.SaveChangesAsync();
            }
        }
    }
}
