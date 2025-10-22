using BookShop.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BookShop.Pages.Order
{
    public class DeleteModel : PageModel
    {
        private readonly IOrderRepository _orderRepository;

        public DeleteModel(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        [BindProperty]
        public Models.Order Order { get; set; } = new();

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null) return RedirectToPage("./Index");
            var order = await _orderRepository.GetOrderByIdAsync(id.Value);
            if (order == null) return RedirectToPage("./Index");
            Order = order;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            await _orderRepository.DeleteOrderAsync(Order.OrderId);
            TempData["SuccessMessage"] = "Order deleted successfully!";
            return RedirectToPage("./Index");
        }
    }
}
