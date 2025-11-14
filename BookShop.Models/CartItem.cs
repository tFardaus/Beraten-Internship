using System.ComponentModel.DataAnnotations;

namespace BookShop.Models
{
    public class CartItem
    {
        [Key]
        public int CartItemId { get; set; }

        [Required]
        public int CartId { get; set; }
        public Cart Cart { get; set; } = null!;

        [Required]
        public int BookId { get; set; }
        public Book Book { get; set; } = null!;

        [Required]
        public int Quantity { get; set; }

        [Required]
        public decimal Price { get; set; }

        public decimal Total => Price * Quantity;
    }
}
