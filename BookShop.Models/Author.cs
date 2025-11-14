using System.ComponentModel.DataAnnotations;

namespace BookShop.Models
{
    public class Author
    {
        [Key]
        public int AuthorId { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        public string Biography { get; set; } = string.Empty;

        public ICollection<Book> Books { get; set; } = new List<Book>();

        public string? AuthorDataJson { get; set; }
    }
}
