using BookShop.Models;

namespace BookShop.Services
{
    public interface IBookRepository
    {
        IEnumerable<Book> GetAllBook();
        Book? GetBookById(int id);
        void AddBook(Book book);
        void UpdateBook(Book book);
        void DeleteBook(int id);
    }
}
