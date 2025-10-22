using BookShop.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BookShop.Pages.Customer
{
    public class EditModel : PageModel
    {
        private readonly ICustomerRepository _customerRepository;

        public EditModel(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        [BindProperty]
        public Models.Customer Customer { get; set; } = new();

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null) return RedirectToPage("./Index");
            var customer = await _customerRepository.GetCustomerByIdAsync(id.Value);
            if (customer == null) return RedirectToPage("./Index");
            Customer = customer;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Please fix the errors in the form.";
                return Page();
            }
            await _customerRepository.UpdateCustomerAsync(Customer);
            TempData["SuccessMessage"] = "Customer updated successfully!";
            return RedirectToPage("./Index");
        }
    }
}
