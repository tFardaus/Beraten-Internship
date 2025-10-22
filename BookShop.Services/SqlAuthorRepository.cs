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

        public async Task<IEnumerable<Author>> GetAllAuthorsAsync()
        {
            return await _context.Authors.AsNoTracking().Include(a => a.Books).ToListAsync();
            
        }

        public async Task<IEnumerable<Author>> SearchAuthorsAsync(string searchTerm)
        {
            return await _context.Authors.AsNoTracking()
                .Include(a => a.Books)
                .Where(a => a.Name.Contains(searchTerm))
                .ToListAsync();
        }

        public async Task<Author?> GetAuthorByIdAsync(int id)
        {
            return await _context.Authors.AsNoTracking().Include(a => a.Books).FirstOrDefaultAsync(a => a.AuthorId == id);
        }

        public async Task AddAuthorAsync(Author author)
        {
            _context.Authors.Add(author);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAuthorAsync(Author author)
        {
            _context.Authors.Update(author);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAuthorAsync(int id)
        {
            var author = await _context.Authors.FindAsync(id);
            if (author != null)
            {
                _context.Authors.Remove(author);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<AuthorWithBooksDto>> GetAuthorWithBooksAsync(int authorId)
        {
            return await _context.AuthorWithBooksResults
                .FromSqlRaw<AuthorWithBooksDto>(" GetAuthorWithBooks {0}", authorId).AsNoTracking()
                .ToListAsync();
        }
    }
}
