using BookShop.Models;
using BookShop.Services;
using Microsoft.AspNetCore.Mvc;

namespace BookShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly IBookRepository _bookRepository;

        public BooksController(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Book book)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                return BadRequest(new { success = false, message = string.Join(", ", errors) });
            }

            try
            {
                await _bookRepository.AddBookAsync(book);
                return Ok(new { success = true, message = "Book created successfully!" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.InnerException?.Message ?? ex.Message });
            }
        }

      
        
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var books = await _bookRepository.GetAllBookAsync();
            return Ok(books);
        }

        
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var book = await _bookRepository.GetBookDetailsAsync(id);
            if (book == null)
            {
                return NotFound(new { message = "Book not found" });
            }
            return Ok(book);
        }

      

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Book book)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            book.BookId = id;
            await _bookRepository.UpdateBookAsync(book);
            return Ok(new { success = true, message = "Book updated successfully!" });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _bookRepository.DeleteBookAsync(id);
            return Ok(new { success = true, message = "Book deleted successfully!" });
        }
    }
}
