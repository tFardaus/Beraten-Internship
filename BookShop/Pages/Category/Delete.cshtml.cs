using BookShop.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BookShop.Pages.Category
{
    public class DeleteModel : PageModel
    {
        private readonly ICategoryRepository _categoryRepository;

        public DeleteModel(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        [BindProperty]
        public Models.Category Category { get; set; } = new();

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null) return RedirectToPage("./Index");
            var category = await _categoryRepository.GetCategoryByIdAsync(id.Value);
            if (category == null) return RedirectToPage("./Index");
            Category = category;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            await _categoryRepository.DeleteCategoryAsync(Category.CategoryId);
            TempData["SuccessMessage"] = "Category deleted successfully!";
            return RedirectToPage("./Index");
        }
    }
}
