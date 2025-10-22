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

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Please fix the errors in the form.";
                return Page();
            }
            await _customerRepository.AddCustomerAsync(Customer);
            TempData["SuccessMessage"] = "Customer created successfully!";
            return RedirectToPage("./Index");
        }
    }
}
