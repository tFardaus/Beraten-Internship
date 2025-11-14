using BookShop.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace BookShop.Services
{
    public class SqlCategoryRepository : ICategoryRepository
    {
        private readonly AppDbContext _context;
        private readonly IMemoryCache _cache;

        public SqlCategoryRepository(AppDbContext context, IMemoryCache cache)
        {
            _context = context;
            _cache = cache;
        }

        public async Task<IEnumerable<Category>> GetAllCategoriesAsync()
        {
            if (_cache.TryGetValue("all_categories", out IEnumerable<Category>? categories))
            {
                return categories!;
            }

            try
            {
                categories = await _context.Categories.AsNoTracking().Include(c => c.Books).ToListAsync();
                
                var cacheOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1),
                    SlidingExpiration = TimeSpan.FromMinutes(5)
                };
                _cache.Set("all_categories", categories, cacheOptions);
                return categories;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<Category>> SearchCategoriesAsync(string searchTerm)
        {
            try
            {
                return await _context.Categories.AsNoTracking()
                    .Include(c => c.Books)
                    .Where(c => c.Name.Contains(searchTerm))
                    .ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Category?> GetCategoryByIdAsync(int id)
        {
            try
            {
                return await _context.Categories.AsNoTracking().Include(c => c.Books).FirstOrDefaultAsync(c => c.CategoryId == id);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task AddCategoryAsync(Category category)
        {
            try
            {
                _context.Categories.Add(category);
                await _context.SaveChangesAsync();
                
                _cache.Remove("all_categories");
                var categories = await _context.Categories.AsNoTracking().Include(c => c.Books).ToListAsync();
                var cacheOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1),
                    SlidingExpiration = TimeSpan.FromMinutes(20)
                };
                _cache.Set("all_categories", categories, cacheOptions);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task UpdateCategoryAsync(Category category)
        {
            try
            {
                _context.Categories.Update(category);
                await _context.SaveChangesAsync();
                
                _cache.Remove("all_categories");
                var categories = await _context.Categories.AsNoTracking().Include(c => c.Books).ToListAsync();
                var cacheOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1),
                    SlidingExpiration = TimeSpan.FromMinutes(20)
                };
                _cache.Set("all_categories", categories, cacheOptions);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task DeleteCategoryAsync(int id)
        {
            try
            {
                var category = await _context.Categories.FindAsync(id);
                if (category != null)
                {
                    _context.Categories.Remove(category);
                    await _context.SaveChangesAsync();
                    
                    _cache.Remove("all_categories");
                    var categories = await _context.Categories.AsNoTracking().Include(c => c.Books).ToListAsync();
                    var cacheOptions = new MemoryCacheEntryOptions
                    {
                        AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1),
                        SlidingExpiration = TimeSpan.FromMinutes(20)
                    };
                    _cache.Set("all_categories", categories, cacheOptions);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
