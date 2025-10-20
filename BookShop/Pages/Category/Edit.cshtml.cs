using BookShop.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BookShop.Pages.Category
{
    public class EditModel : PageModel
    {
        private readonly ICategoryRepository _categoryRepository;

        public EditModel(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        [BindProperty]
        public Models.Category Category { get; set; } = new();

        public IActionResult OnGet(int? id)
        {
            if (id == null) return RedirectToPage("./Index");
            var category = _categoryRepository.GetCategoryById(id.Value);
            if (category == null) return RedirectToPage("./Index");
            Category = category;
            return Page();
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid) return Page();
            _categoryRepository.UpdateCategory(Category);
            return RedirectToPage("./Index");
        }
    }
}
