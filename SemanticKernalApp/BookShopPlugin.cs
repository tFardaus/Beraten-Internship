using Microsoft.SemanticKernel;
using System.ComponentModel;

namespace SemanticKernalApp
{
    public class BookShopPlugin
    {
        private readonly string _connectionString;

        public BookShopPlugin(string connectionString)
        {
            _connectionString = connectionString;
        }

        [KernelFunction]
        [Description("Gets total num of books")]
        public int GetBookCount()
        {
            using var db = new BookShopDbContext(_connectionString);
            return db.Books.Count();
        }

        [KernelFunction]
        [Description("Gets total num of authors")]
        public int GetAuthorCount()
        {
            using var db = new BookShopDbContext(_connectionString);
            return db.Authors.Count();
        }

        [KernelFunction]
        [Description("Gets total num of categories")]
        public int GetCategoryCount()
        {
            using var db = new BookShopDbContext(_connectionString);
            return db.Categories.Count();
        }

        [KernelFunction]
        [Description("Gets all book titles")]
        public string GetAllBooks()
        {
            using var db = new BookShopDbContext(_connectionString);
            var titles = db.Books.Select(b => b.Title).ToList();
            return string.Join(", ", titles);
        }

        [KernelFunction]
        [Description("Gets all author names")]
        public string GetAllAuthors()
        {
            using var db = new BookShopDbContext(_connectionString);
            var names = db.Authors.Select(a => a.Name).ToList();
            return string.Join(", ", names);
        }
    }
}
