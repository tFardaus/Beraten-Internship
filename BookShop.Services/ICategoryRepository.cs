using BookShop.Models;

namespace BookShop.Services
{
    public interface ICategoryRepository
    {
        IEnumerable<Category> GetAllCategories();
        IEnumerable<Category> SearchCategories(string searchTerm);
        Category? GetCategoryById(int id);
        void AddCategory(Category category);
        void UpdateCategory(Category category);
        void DeleteCategory(int id);
    }
}
