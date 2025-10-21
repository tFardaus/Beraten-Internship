namespace BookShop.Models
{
    public class AuthorWithBooksDto
    {
        public int AuthorId { get; set; }
        public string AuthorName { get; set; } = string.Empty;
        public string Biography { get; set; } = string.Empty;
        public int? BookId { get; set; }
        public string? BookTitle { get; set; }
        public string? BookDescription { get; set; }
        public decimal? Price { get; set; }
        public int? Stock { get; set; }
    }
}
