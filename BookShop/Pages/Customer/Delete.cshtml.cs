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

        public IActionResult OnGet(int? id)
        {
            if (id == null) return RedirectToPage("./Index");
            var customer = _customerRepository.GetCustomerById(id.Value);
            if (customer == null) return RedirectToPage("./Index");
            Customer = customer;
            return Page();
        }

        public IActionResult OnPost()
        {
            _customerRepository.DeleteCustomer(Customer.CustomerId);
            return RedirectToPage("./Index");
        }
    }
}
