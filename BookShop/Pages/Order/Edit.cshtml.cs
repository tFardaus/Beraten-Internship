using BookShop.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BookShop.Pages.Order
{
    public class EditModel : PageModel
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ICustomerRepository _customerRepository;

        public EditModel(IOrderRepository orderRepository, ICustomerRepository customerRepository)
        {
            _orderRepository = orderRepository;
            _customerRepository = customerRepository;
        }

        [BindProperty]
        public Models.Order Order { get; set; } = new();

        public SelectList Customers { get; set; }

        public IActionResult OnGet(int? id)
        {
            if (id == null) return RedirectToPage("./Index");
            var order = _orderRepository.GetOrderById(id.Value);
            if (order == null) return RedirectToPage("./Index");
            Order = order;
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
            _orderRepository.UpdateOrder(Order);
            return RedirectToPage("./Index");
        }
    }
}
