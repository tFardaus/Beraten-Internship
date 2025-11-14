using Microsoft.EntityFrameworkCore;

namespace SemanticKernalApp
{
    public class BookShopDbContext : DbContext
    {
        private readonly string _connectionString;

        public BookShopDbContext(string connectionString)
        {
            _connectionString = connectionString;
        }

        public DbSet<Book> Books { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<Category> Categories { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_connectionString);
        }
    }
}
