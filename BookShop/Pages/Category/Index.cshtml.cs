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

        public void OnGet()
        {
            Categories = _categoryRepository.GetAllCategories().ToList();
        }
    }
}
