using BookShop.Models;
using BookShop.Services;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BookShop.Pages.Category
{
    public class IndexModel : PageModel
    {
        private readonly ICategoryRepository _categoryRepository;

        public IndexModel(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public List<Models.Category> Categories { get; set; } = new List<Models.Category>();
        public string SearchTerm { get; set; } = string.Empty;

        public void OnGet(string searchTerm)
        {
            SearchTerm = searchTerm ?? string.Empty;
            
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                Categories = _categoryRepository.GetAllCategories().ToList();
            }
            else
            {
                Categories = _categoryRepository.SearchCategories(searchTerm).ToList();
            }
        }
    }
}
