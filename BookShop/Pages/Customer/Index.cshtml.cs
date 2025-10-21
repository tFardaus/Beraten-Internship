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
        public string SearchTerm { get; set; } = string.Empty;

        public void OnGet(string searchTerm)
        {
            SearchTerm = searchTerm ?? string.Empty;
            
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                Customers = _customerRepository.GetAllCustomers().ToList();
            }
            else
            {
                Customers = _customerRepository.SearchCustomers(searchTerm).ToList();
            }
        }
    }
}
