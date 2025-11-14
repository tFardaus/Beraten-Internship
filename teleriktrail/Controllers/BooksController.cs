using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;
using teleriktrail.Data;

namespace teleriktrail.Controllers
{
    public class BooksController : Controller
    {
        private readonly BookDbContext _db;

        public BooksController(BookDbContext db)
        {
            _db = db;
        }

        [HttpPost("api/Books/Read")]
        public JsonResult Read([DataSourceRequest] DataSourceRequest request)
        {
            var books = _db.Books.ToList();
            return Json(books.ToDataSourceResult(request));
        }
    }
}
