using BookShop.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BookShop.Pages.Customer
{
    public class DeleteModel : PageModel
    {
        private readonly ICustomerRepository _customerRepository;

        public DeleteModel(ICustomerRepository customerRepository)
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
            await _customerRepository.DeleteCustomerAsync(Customer.CustomerId);
            TempData["SuccessMessage"] = "Customer deleted successfully!";
            return RedirectToPage("./Index");
        }
    }
}
