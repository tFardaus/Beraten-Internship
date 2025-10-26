using Microsoft.AspNetCore.Mvc;

namespace BookShop.ViewComponents
{
    public class PriceSortFilterViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(string currentSortOrder, string currentSearchTerm)
        {
            ViewData["CurrentSortOrder"] = currentSortOrder ?? string.Empty;
            ViewData["CurrentSearchTerm"] = currentSearchTerm ?? string.Empty;
            return View();
        }
    }
}
