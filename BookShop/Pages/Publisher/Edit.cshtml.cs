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

        public IActionResult OnGet(int? id)
        {
            if (id == null) return RedirectToPage("./Index");
            var publisher = _publisherRepository.GetPublisherById(id.Value);
            if (publisher == null) return RedirectToPage("./Index");
            Publisher = publisher;
            return Page();
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid) return Page();
            _publisherRepository.UpdatePublisher(Publisher);
            return RedirectToPage("./Index");
        }
    }
}
