using BookShop.Services;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BookShop.Pages.Customer
{
    public class IndexModel : PageModel
    {
        private readonly ICustomerRepository _customerRepository;

        public IndexModel(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public List<Models.Customer> Customers { get; set; } = new();

        public void OnGet()
        {
            Customers = _customerRepository.GetAllCustomers().ToList();
        }
    }
}
