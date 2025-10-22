using BookShop.Models;
using BookShop.Services;
using Microsoft.AspNetCore.Mvc;

namespace BookShop.ViewComponents
{
    public class CategoryStatsViewComponent : ViewComponent
    {
        private readonly ICategoryRepository categoryRepository;

        public CategoryStatsViewComponent(ICategoryRepository categoryRepository)
        {
            this.categoryRepository = categoryRepository;
        }

        public async Task <IViewComponentResult> InvokeAsync(Category? Name)
        {
            var categories = (await categoryRepository.GetAllCategoriesAsync())
                .Select(c => new
                {
                    CategoryName = c.Name,
                    BookCount = c.Books.Count
                })
                .OrderByDescending(c => c.BookCount)
                .ToList();
            
            return View(categories);
        }
    }
}
