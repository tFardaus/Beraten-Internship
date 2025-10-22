using BookShop.Models;
using Microsoft.EntityFrameworkCore;

namespace BookShop.Services
{
    public class SqlBookRepository : IBookRepository
    {
        private readonly AppDbContext _context;

        public SqlBookRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Book>> GetAllBookAsync()
        {
            return await _context.Books.AsNoTracking()
                .Include(b => b.Author)
                .Include(b => b.Category)
                .Include(b => b.Publisher)
                .ToListAsync();
        }

        public async Task<IEnumerable<Book>> SearchBooksAsync(string searchTerm)
        {
            return await _context.Books.AsNoTracking()
                .Include(b => b.Author)
                .Include(b => b.Category)
                .Include(b => b.Publisher)
                .Where(b => b.BookTitle.Contains(searchTerm))
                .ToListAsync();
        }

        public async Task<Book?> GetBookByIdAsync(int id)
        {
            return await _context.Books.AsNoTracking()
                .Include(b => b.Author)
                .Include(b => b.Category)
                .Include(b => b.Publisher)
                .FirstOrDefaultAsync(b => b.BookId == id);
        }

        public async Task<BookDetailsDto?> GetBookDetailsAsync(int id)
        {
            return await _context.Books.AsNoTracking()
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

        public async Task AddBookAsync(Book book)
        {
            _context.Books.Add(book);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateBookAsync(Book book)
        {
            _context.Books.Update(book);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteBookAsync(int id)
        {
            var book = await _context.Books.FindAsync(id);
            if (book != null)
            {
                _context.Books.Remove(book);
                await _context.SaveChangesAsync();
            }
        }
    }
}
