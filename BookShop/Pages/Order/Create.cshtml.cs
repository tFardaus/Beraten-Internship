using BookShop.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BookShop.Pages.Order
{
    public class CreateModel : PageModel
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ICustomerRepository _customerRepository;

        public CreateModel(IOrderRepository orderRepository, ICustomerRepository customerRepository)
        {
            _orderRepository = orderRepository;
            _customerRepository = customerRepository;
        }

        [BindProperty]
        public Models.Order Order { get; set; } = new();

        public SelectList Customers { get; set; }

        public IActionResult OnGet()
        {
            Customers = new SelectList(_customerRepository.GetAllCustomers(), "CustomerId", "Name");
            return Page();
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                Customers = new SelectList(_customerRepository.GetAllCustomers(), "CustomerId", "Name");
                return Page();
            }
            _orderRepository.AddOrder(Order);
            return RedirectToPage("./Index");
        }
    }
}
