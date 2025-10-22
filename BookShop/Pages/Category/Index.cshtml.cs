using BookShop.Models;
using BookShop.Services;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BookShop.Pages.Category
{
    public class IndexModel : PageModel
    {
        private readonly ICategoryRepository categoryRepository;

        public IndexModel(ICategoryRepository categoryRepository)
        {
            this.categoryRepository = categoryRepository;
        }

        public List<Models.Category> Categories { get; set; } = new List<Models.Category>();
        public string SearchTerm { get; set; } = string.Empty;

        public async Task OnGetAsync(string searchTerm)
        {
            SearchTerm = searchTerm ?? string.Empty;
            
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                Categories = (await categoryRepository.GetAllCategoriesAsync()).ToList();
            }
            else
            {
                Categories = (await categoryRepository.SearchCategoriesAsync(searchTerm)).ToList();
            }
        }
    }
}
