using System.ComponentModel.DataAnnotations;

namespace BookShop.Models
{
    public class Cart
    {
        [Key]
        public int CartId { get; set; }

        [Required]
        public string UserId { get; set; } = string.Empty;

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public string? CartItemsJson { get; set; }
    }
}
