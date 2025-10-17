using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShop.Models
{
    public class Book
    {
        [Key]
        public int BookId { get; set; }

        [Required]
        public string BookTitle { get; set; } = string.Empty;

        [Required]
        public string BookDescription { get; set; } = string.Empty;

        [Required]
        public string BookAuthor { get; set; } = string.Empty;
    }
}
