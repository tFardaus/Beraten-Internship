using BookShop.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BookShop.Pages.Publisher
{
    public class DeleteModel : PageModel
    {
        private readonly IPublisherRepository _publisherRepository;

        public DeleteModel(IPublisherRepository publisherRepository)
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
            await _publisherRepository.DeletePublisherAsync(Publisher.PublisherId);
            TempData["SuccessMessage"] = "Publisher deleted successfully!";
            return RedirectToPage("./Index");
        }
    }
}
