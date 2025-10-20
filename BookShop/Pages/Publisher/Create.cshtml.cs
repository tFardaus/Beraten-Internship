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

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid) return Page();
            _publisherRepository.AddPublisher(Publisher);
            return RedirectToPage("./Index");
        }
    }
}
