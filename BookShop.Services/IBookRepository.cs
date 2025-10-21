using BookShop.Models;

namespace BookShop.Services
{
    public interface IBookRepository
    {
        IEnumerable<Book> GetAllBook();
        IEnumerable<Book> SearchBooks(string searchTerm);
        Book? GetBookById(int id);
        BookDetailsDto? GetBookDetails(int id);
        void AddBook(Book book);
        void UpdateBook(Book book);
        void DeleteBook(int id);
    }
}
