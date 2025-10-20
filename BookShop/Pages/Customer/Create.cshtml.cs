using BookShop.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BookShop.Pages.Customer
{
    public class CreateModel : PageModel
    {
        private readonly ICustomerRepository _customerRepository;

        public CreateModel(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        [BindProperty]
        public Models.Customer Customer { get; set; } = new();

        public IActionResult OnGet() => Page();

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid) return Page();
            _customerRepository.AddCustomer(Customer);
            return RedirectToPage("./Index");
        }
    }
}
