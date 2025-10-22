using BookShop.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BookShop.Pages.Publisher
{
    public class EditModel : PageModel
    {
        private readonly IPublisherRepository _publisherRepository;

        public EditModel(IPublisherRepository publisherRepository)
        {
            _publisherRepository = publisherRepository;
        }

        [BindProperty]
        public Models.Publisher Publisher { get; set; } = new();

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null) return RedirectToPage("./Index");
            var publisher = await _publisherRepository.GetPublisherByIdAsync(id.Value);
            if (publisher == null) return RedirectToPage("./Index");
            Publisher = publisher;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Please fix the errors in the form.";
                return Page();
            }
            await _publisherRepository.UpdatePublisherAsync(Publisher);
            TempData["SuccessMessage"] = "Publisher updated successfully!";
            return RedirectToPage("./Index");
        }
    }
}
