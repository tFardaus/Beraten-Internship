using System.ComponentModel.DataAnnotations.Schema;

namespace teleriktrail.Models
{
    [Table("Books")]
    public class Book
    {
        public int BookId { get; set; }
        public string BookTitle { get; set; } = string.Empty;
        public string BookDescription { get; set; } = string.Empty;
        public int AuthorId { get; set; }
        public int CategoryId { get; set; }
        public decimal Price { get; set; }
        public int PublisherId { get; set; }
        public int Stock { get; set; }
    }
}
