using BookShop.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace BookShop.Services
{
    public class SqlBookRepository : IBookRepository
    {
        private readonly AppDbContext context;
        private readonly IMemoryCache _cache;
        private static SemaphoreSlim _semaphore = new SemaphoreSlim(5, 5);

        public SqlBookRepository(AppDbContext context, IMemoryCache cache)
        {
            this.context = context;
            _cache = cache;
        }

        public async Task<IEnumerable<Book>> GetAllBookAsync()
        {
            if (_cache.TryGetValue("all_books", out IEnumerable<Book>? books))
            {
                return books!;
            }


            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(5));
            await _semaphore.WaitAsync(cts.Token);
            try
            {
                
                await Task.Delay(10000, cts.Token);//Testing cancellation token
                
                books = await this.context.Books.AsNoTracking()
                    .Include(b => b.Author)
                    .Include(b => b.Category)
                    .Include(b => b.Publisher)
                    .ToListAsync(cts.Token);

                var cacheOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1),
                    SlidingExpiration = TimeSpan.FromMinutes(5)
                };
                _cache.Set("all_books", books, cacheOptions);
                return books;
            }
            finally
            {
                _semaphore.Release();
            }
        }
         
        public async Task<IEnumerable<Book>> SearchBooksAsync(string searchTerm)
        {
            try
            {
                return await this.context.Books.AsNoTracking()
                    .Include(b => b.Author)
                    .Include(b => b.Category)
                    .Include(b => b.Publisher)
                    .Where(b => b.BookTitle.Contains(searchTerm))
                    .ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Book?> GetBookByIdAsync(int id)
        {
            await _semaphore.WaitAsync();
            try
            {
                return await this.context.Books.AsNoTracking()
                    .Include(b => b.Author)
                    .Include(b => b.Category)
                    .Include(b => b.Publisher)
                    .FirstOrDefaultAsync(b => b.BookId == id);
            }
            finally
            {
                _semaphore.Release();
            }
        }

        public async Task<BookDetailsDto?> GetBookDetailsAsync(int id)
        {
            try
            {
                return await this.context.Books.AsNoTracking()
                    .Where(b => b.BookId == id)
                    .Select(b => new BookDetailsDto
                    {
                        BookId = b.BookId,
                        BookTitle = b.BookTitle,
                        BookDescription = b.BookDescription,
                        Price = b.Price,
                        Stock = b.Stock,
                        AuthorName = b.Author!.Name,
                        AuthorBiography = b.Author.Biography,
                        PublisherName = b.Publisher!.Name,
                        PublisherAddress = b.Publisher.Address,
                        PublisherPhone = b.Publisher.Phone,
                        CategoryName = b.Category!.Name,
                        CategoryDescription = b.Category.Description
                    })
                    .FirstOrDefaultAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task AddBookAsync(Book book)
        {
            await _semaphore.WaitAsync();
            try
            {
                this.context.Books.Add(book);
                await this.context.SaveChangesAsync();
                
                _cache.Remove("all_books");
                var books = await this.context.Books.AsNoTracking()
                    .Include(b => b.Author)
                    .Include(b => b.Category)
                    .Include(b => b.Publisher)
                    .ToListAsync();
                var cacheOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1),
                    SlidingExpiration = TimeSpan.FromMinutes(20)
                };
                _cache.Set("all_books", books, cacheOptions);
            }
            finally
            {
                _semaphore.Release();
            }
        }

        public async Task UpdateBookAsync(Book book)
        {
            try
            {
                this.context.Books.Update(book);
                await this.context.SaveChangesAsync();
                
                _cache.Remove("all_books");
                var books = await this.context.Books.AsNoTracking()
                    .Include(b => b.Author)
                    .Include(b => b.Category)
                    .Include(b => b.Publisher)
                    .ToListAsync();
                var cacheOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1),
                    SlidingExpiration = TimeSpan.FromMinutes(20)
                };
                _cache.Set("all_books", books, cacheOptions);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task DeleteBookAsync(int id)
        {
            try
            {
                var book = await this.context.Books.FindAsync(id);
                if (book != null)
                {
                    this.context.Books.Remove(book);
                    await this.context.SaveChangesAsync();
                    
                    _cache.Remove("all_books");
                    var books = await this.context.Books.AsNoTracking()
                        .Include(b => b.Author)
                        .Include(b => b.Category)
                        .Include(b => b.Publisher)
                        .ToListAsync();
                    var cacheOptions = new MemoryCacheEntryOptions
                    {
                        AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1),
                        SlidingExpiration = TimeSpan.FromMinutes(20)
                    };
                    _cache.Set("all_books", books, cacheOptions);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
