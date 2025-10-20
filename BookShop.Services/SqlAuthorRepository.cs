using BookShop.Models;
using Microsoft.EntityFrameworkCore;

namespace BookShop.Services
{
    public class SqlAuthorRepository : IAuthorRepository
    {
        private readonly AppDbContext _context;

        public SqlAuthorRepository(AppDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Author> GetAllAuthors()
        {
            return _context.Authors.Include(a => a.Books).ToList();
        }

        public Author? GetAuthorById(int id)
        {
            return _context.Authors.Include(a => a.Books).FirstOrDefault(a => a.AuthorId == id);
        }

        public void AddAuthor(Author author)
        {
            _context.Authors.Add(author);
            _context.SaveChanges();
        }

        public void UpdateAuthor(Author author)
        {
            _context.Authors.Update(author);
            _context.SaveChanges();
        }

        public void DeleteAuthor(int id)
        {
            var author = _context.Authors.Find(id);
            if (author != null)
            {
                _context.Authors.Remove(author);
                _context.SaveChanges();
            }
        }
    }
}
