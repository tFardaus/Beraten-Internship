using BookShop.Models;

namespace BookShop.Services
{
    public interface IAuthorRepository
    {
        Task<IEnumerable<Author>> GetAllAuthorsAsync();
        Task<IEnumerable<Author>> SearchAuthorsAsync(string searchTerm);
        Task<Author?> GetAuthorByIdAsync(int id);
        Task<IEnumerable<AuthorWithBooksDto>> GetAuthorWithBooksAsync(int authorId);
        Task AddAuthorAsync(Author author);
        Task UpdateAuthorAsync(Author author);
        Task DeleteAuthorAsync(int id);
    }
}
