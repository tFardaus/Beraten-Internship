using BookShop.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BookShop.Pages.Publisher
{
    public class CreateModel : PageModel
    {
        private readonly IPublisherRepository _publisherRepository;

        public CreateModel(IPublisherRepository publisherRepository)
        {
            _publisherRepository = publisherRepository;
        }

        [BindProperty]
        public Models.Publisher Publisher { get; set; } = new();

        public IActionResult OnGet() => Page();

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Please fix the errors in the form.";
                return Page();
            }
            await _publisherRepository.AddPublisherAsync(Publisher);
            TempData["SuccessMessage"] = "Publisher created successfully!";
            return RedirectToPage("./Index");
        }
    }
}
