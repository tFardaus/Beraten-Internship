using BookShop.Models;
using BookShop.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BookShop.Pages.Author
{
    public class EditModel : PageModel
    {
        private readonly IAuthorRepository _authorRepository;

        public EditModel(IAuthorRepository authorRepository)
        {
            _authorRepository = authorRepository;
        }

        [BindProperty]
        public Models.Author Author { get; set; } = new Models.Author();

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return RedirectToPage("./Index");
            }

            var author = await _authorRepository.GetAuthorByIdAsync(id.Value);
            if (author == null)
            {
                return RedirectToPage("./Index");
            }

            Author = author;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Please fix the errors in the form.";
                return Page();
            }

            await _authorRepository.UpdateAuthorAsync(Author);
            TempData["SuccessMessage"] = "Author updated successfully!";
            return RedirectToPage("./Index");
        }
    }
}
