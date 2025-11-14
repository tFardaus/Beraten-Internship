using BookShop.Models;
using Microsoft.EntityFrameworkCore;
using BookShop.Services.Extensions;
using Microsoft.Extensions.Caching.Memory;

namespace BookShop.Services
{
    public class SqlPublisherRepository : IPublisherRepository
    {
        private readonly AppDbContext _context;
        private readonly IMemoryCache _cache;

        public SqlPublisherRepository(AppDbContext context, IMemoryCache cache)
        {
            _context = context;
            _cache = cache;
        }

        public async Task<IEnumerable<Publisher>> GetAllPublishersAsync()
        {
            if (_cache.TryGetValue("all_publishers", out IEnumerable<Publisher>? publishers))
            {
                return publishers!;
            }

            try
            {
                publishers = await _context.Publishers.AsNoTracking().Include(p => p.Books).ToListAsync();
                
                var cacheOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1),
                    SlidingExpiration = TimeSpan.FromMinutes(5)
                };
                _cache.Set("all_publishers", publishers, cacheOptions);
                return publishers;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<Publisher>> SearchPublishersAsync(string searchTerm)
        {
            try
            {
                return await _context.Publishers.AsNoTracking()
                    .Include(p => p.Books)
                    .Where(p => p.Name.Contains(searchTerm))
                    .ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Publisher?> GetPublisherByIdAsync(int id)
        {
            try
            {
                return await _context.Publishers.AsNoTracking().Include(p => p.Books).FirstOrDefaultAsync(p => p.PublisherId == id);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task AddPublisherAsync(Publisher publisher)
        {
            try
            {
                publisher.PublisherDataXml = publisher.ToXml();
                publisher.Name = string.Empty;
                publisher.Address = string.Empty;
                publisher.Phone = string.Empty;
                
                _context.Publishers.Add(publisher);
                await _context.SaveChangesAsync();
                
                _cache.Remove("all_publishers");
                var publishers = await _context.Publishers.AsNoTracking().Include(p => p.Books).ToListAsync();
                var cacheOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1),
                    SlidingExpiration = TimeSpan.FromMinutes(20)
                };
                _cache.Set("all_publishers", publishers, cacheOptions);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task UpdatePublisherAsync(Publisher publisher)
        {
            try
            {
                publisher.PublisherDataXml = publisher.ToXml();
                publisher.Name = string.Empty;
                publisher.Address = string.Empty;
                publisher.Phone = string.Empty;
                
                _context.Publishers.Update(publisher);
                await _context.SaveChangesAsync();
                
                _cache.Remove("all_publishers");
                var publishers = await _context.Publishers.AsNoTracking().Include(p => p.Books).ToListAsync();
                var cacheOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1),
                    SlidingExpiration = TimeSpan.FromMinutes(20)
                };
                _cache.Set("all_publishers", publishers, cacheOptions);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task DeletePublisherAsync(int id)
        {
            try
            {
                var publisher = await _context.Publishers.FindAsync(id);
                if (publisher != null)
                {
                    _context.Publishers.Remove(publisher);
                    await _context.SaveChangesAsync();
                    
                    _cache.Remove("all_publishers");
                    var publishers = await _context.Publishers.AsNoTracking().Include(p => p.Books).ToListAsync();
                    var cacheOptions = new MemoryCacheEntryOptions
                    {
                        AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1),
                        SlidingExpiration = TimeSpan.FromMinutes(20)
                    };
                    _cache.Set("all_publishers", publishers, cacheOptions);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
