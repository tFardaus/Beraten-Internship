using Microsoft.EntityFrameworkCore;
using teleriktrail.Models;

namespace teleriktrail.Data
{
    public class BookDbContext : DbContext
    {
        public BookDbContext(DbContextOptions<BookDbContext> options) : base(options) { }
        
        public DbSet<Book> Books { get; set; }
    }
}
