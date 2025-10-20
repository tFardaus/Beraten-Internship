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

        public IActionResult OnGet(int? id)
        {
            if (id == null) return RedirectToPage("./Index");
            var order = _orderRepository.GetOrderById(id.Value);
            if (order == null) return RedirectToPage("./Index");
            Order = order;
            return Page();
        }

        public IActionResult OnPost()
        {
            _orderRepository.DeleteOrder(Order.OrderId);
            return RedirectToPage("./Index");
        }
    }
}
