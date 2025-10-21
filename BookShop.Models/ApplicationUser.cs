using Microsoft.AspNetCore.Identity;

namespace BookShop.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; } = string.Empty;
    }
}
