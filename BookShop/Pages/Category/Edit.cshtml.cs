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
            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Please fix the errors in the form.";
                return Page();
            }
            await _categoryRepository.UpdateCategoryAsync(Category);
            TempData["SuccessMessage"] = "Category updated successfully!";
            return RedirectToPage("./Index");
        }
    }
}
