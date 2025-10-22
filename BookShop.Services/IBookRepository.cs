using BookShop.Models;

namespace BookShop.Services
{
    public interface IBookRepository
    {
        Task<IEnumerable<Book>> GetAllBookAsync();
        Task<IEnumerable<Book>> SearchBooksAsync(string searchTerm);
        Task<Book?> GetBookByIdAsync(int id);
        Task<BookDetailsDto?> GetBookDetailsAsync(int id);
        Task AddBookAsync(Book book);
        Task UpdateBookAsync(Book book);
        Task DeleteBookAsync(int id);
    }
}
