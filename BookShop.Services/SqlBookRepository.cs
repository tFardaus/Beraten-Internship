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

        public IEnumerable<Book> GetAllBook()
        {
            return _context.Books
                .Include(b => b.Author)
                .Include(b => b.Category)
                .Include(b => b.Publisher)
                .ToList();
        }

        public Book? GetBookById(int id)
        {
            return _context.Books
                .Include(b => b.Author)
                .Include(b => b.Category)
                .Include(b => b.Publisher)
                .FirstOrDefault(b => b.BookId == id);
        }

        public BookDetailsDto? GetBookDetails(int id)
        {
            return _context.Books
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
                .FirstOrDefault();
        }

        public void AddBook(Book book)
        {
            _context.Books.Add(book);
            _context.SaveChanges();
        }

        public void UpdateBook(Book book)
        {
            _context.Books.Update(book);
            _context.SaveChanges();
        }

        public void DeleteBook(int id)
        {
            var book = _context.Books.Find(id);
            if (book != null)
            {
                _context.Books.Remove(book);
                _context.SaveChanges();
            }
        }
    }
}
