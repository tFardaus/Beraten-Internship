using BookShop.Models;
using Microsoft.EntityFrameworkCore;
using BookShop.Services.Extensions;
using Microsoft.Extensions.Caching.Memory;

namespace BookShop.Services
{
    public class SqlAuthorRepository : IAuthorRepository
    {
        private readonly AppDbContext context;
        private readonly IMemoryCache _cache;

        public SqlAuthorRepository(AppDbContext context, IMemoryCache cache)
        {
            this.context = context;
            _cache = cache;
        }

        public async Task<IEnumerable<Author>> GetAllAuthorsAsync()
        {
            if (_cache.TryGetValue("all_authors", out IEnumerable<Author>? authors))
            {
                return authors!;
            }

            try
            {
                authors = await this.context.Authors.AsNoTracking().Include(a => a.Books).ToListAsync();
                
                var cacheOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1),
                    SlidingExpiration = TimeSpan.FromMinutes(5)
                };
                _cache.Set("all_authors", authors, cacheOptions);
                return authors;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<Author>> SearchAuthorsAsync(string searchTerm)
        {
            try
            {
                return await this.context.Authors.AsNoTracking()
                    .Include(a => a.Books)
                    .Where(a => a.Name.Contains(searchTerm))
                    .ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Author?> GetAuthorByIdAsync(int id)
        {
            try
            {
                return await this.context.Authors.AsNoTracking().Include(a => a.Books).FirstOrDefaultAsync(a => a.AuthorId == id);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task AddAuthorAsync(Author author)
        {
            try
            {
                author.AuthorDataJson = author.ToJson();
                author.Name = string.Empty;
                author.Biography = string.Empty;
                
                this.context.Authors.Add(author);
                await this.context.SaveChangesAsync();
                
                _cache.Remove("all_authors");
                var authors = await this.context.Authors.AsNoTracking().Include(a => a.Books).ToListAsync();
                var cacheOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1),
                    SlidingExpiration = TimeSpan.FromMinutes(20)
                };
                _cache.Set("all_authors", authors, cacheOptions);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task UpdateAuthorAsync(Author author)
        {
            try
            {
                author.AuthorDataJson = author.ToJson();
                author.Name = string.Empty;
                author.Biography = string.Empty;
                
                this.context.Authors.Update(author);
                await this.context.SaveChangesAsync();
                
                _cache.Remove("all_authors");
                var authors = await this.context.Authors.AsNoTracking().Include(a => a.Books).ToListAsync();
                var cacheOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1),
                    SlidingExpiration = TimeSpan.FromMinutes(20)
                };
                _cache.Set("all_authors", authors, cacheOptions);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task DeleteAuthorAsync(int id)
        {
            try
            {
                var author = await this.context.Authors.FindAsync(id);
                if (author != null)
                {
                    this.context.Authors.Remove(author);
                    await this.context.SaveChangesAsync();
                    
                    _cache.Remove("all_authors");
                    var authors = await this.context.Authors.AsNoTracking().Include(a => a.Books).ToListAsync();
                    var cacheOptions = new MemoryCacheEntryOptions
                    {
                        AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1),
                        SlidingExpiration = TimeSpan.FromMinutes(20)
                    };
                    _cache.Set("all_authors", authors, cacheOptions);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<AuthorWithBooksDto>> GetAuthorWithBooksAsync(int authorId)
        {
            try
            {
                return await this.context.AuthorWithBooksResults
                    .FromSqlRaw<AuthorWithBooksDto>(" GetAuthorWithBooks {0}", authorId).AsNoTracking()
                    .ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
