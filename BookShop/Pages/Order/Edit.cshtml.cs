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

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null) return RedirectToPage("./Index");
            var order = await _orderRepository.GetOrderByIdAsync(id.Value);
            if (order == null) return RedirectToPage("./Index");
            Order = order;
            Customers = new SelectList(await _customerRepository.GetAllCustomersAsync(), "CustomerId", "Name");
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Please fix the errors in the form.";
                Customers = new SelectList(await _customerRepository.GetAllCustomersAsync(), "CustomerId", "Name");
                return Page();
            }
            await _orderRepository.UpdateOrderAsync(Order);
            TempData["SuccessMessage"] = "Order updated successfully!";
            return RedirectToPage("./Index");
        }
    }
}
