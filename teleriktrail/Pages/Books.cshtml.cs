using Microsoft.AspNetCore.Mvc.RazorPages;
using teleriktrail.Data;
using teleriktrail.Models;

namespace teleriktrail.Pages
{
    public class BooksModel : PageModel
    {
        private readonly BookDbContext _db;

        public BooksModel(BookDbContext db)
        {
            _db = db;
        }

        public List<Book> Books { get; set; } = new();

        public void OnGet()
        {
            Books = _db.Books.ToList();
        }
    }
}
