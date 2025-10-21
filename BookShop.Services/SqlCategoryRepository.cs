using BookShop.Models;
using Microsoft.EntityFrameworkCore;

namespace BookShop.Services
{
    public class SqlCategoryRepository : ICategoryRepository
    {
        private readonly AppDbContext _context;

        public SqlCategoryRepository(AppDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Category> GetAllCategories()
        {
            return _context.Categories.Include(c => c.Books).ToList();
        }

        public IEnumerable<Category> SearchCategories(string searchTerm)
        {
            return _context.Categories
                .Include(c => c.Books)
                .Where(c => c.Name.Contains(searchTerm))
                .ToList();
        }

        public Category? GetCategoryById(int id)
        {
            return _context.Categories.Include(c => c.Books).FirstOrDefault(c => c.CategoryId == id);
        }

        public void AddCategory(Category category)
        {
            _context.Categories.Add(category);
            _context.SaveChanges();
        }

        public void UpdateCategory(Category category)
        {
            _context.Categories.Update(category);
            _context.SaveChanges();
        }

        public void DeleteCategory(int id)
        {
            var category = _context.Categories.Find(id);
            if (category != null)
            {
                _context.Categories.Remove(category);
                _context.SaveChanges();
            }
        }
    }
}
