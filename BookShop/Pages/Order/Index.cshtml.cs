using BookShop.Services;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BookShop.Pages.Order
{
    public class IndexModel : PageModel
    {
        private readonly IOrderRepository _orderRepository;

        public IndexModel(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public List<Models.Order> Orders { get; set; } = new();

        public async Task OnGetAsync()
        {
            Orders = (await _orderRepository.GetAllOrdersAsync()).ToList();
        }
    }
}
