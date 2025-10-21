namespace BookShop.Models
{
    public class BookDetailsDto
    {
        public int BookId { get; set; }
        public string BookTitle { get; set; } = string.Empty;
        public string BookDescription { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int Stock { get; set; }
        
        //author info
        public string AuthorName { get; set; } = string.Empty;
        public string AuthorBiography { get; set; } = string.Empty;
        
        //Publisher info
        public string PublisherName { get; set; } = string.Empty;
        public string PublisherAddress { get; set; } = string.Empty;
        public string PublisherPhone { get; set; } = string.Empty;
        
        //category info
        public string CategoryName { get; set; } = string.Empty;
        public string CategoryDescription { get; set; } = string.Empty;
    }
}
