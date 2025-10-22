using BookShop.Models;
using BookShop.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BookShop.Pages.Author
{
    public class CreateModel : PageModel
    {
        private readonly IAuthorRepository _authorRepository;

        public CreateModel(IAuthorRepository authorRepository)
        {
            _authorRepository = authorRepository;
        }

        [BindProperty]
        public Models.Author Author { get; set; } = new Models.Author();

        public IActionResult OnGet()
        {
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Please fix the errors in the form.";
                return Page();
            }

            await _authorRepository.AddAuthorAsync(Author);
            TempData["SuccessMessage"] = "Author created successfully!";
            return RedirectToPage("./Index");
        }
    }
}
