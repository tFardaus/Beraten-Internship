using BookShop.Models;

namespace BookShop.Services
{
    public interface IAuthorRepository
    {
        IEnumerable<Author> GetAllAuthors();
        IEnumerable<Author> SearchAuthors(string searchTerm);
        Author? GetAuthorById(int id);
        IEnumerable<AuthorWithBooksDto> GetAuthorWithBooks(int authorId);
        void AddAuthor(Author author);
        void UpdateAuthor(Author author);
        void DeleteAuthor(int id);
    }
}
