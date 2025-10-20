using BookShop.Models;
using BookShop.Services;
using Microsoft.AspNetCore.Mvc;

namespace BookShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorsController : ControllerBase
    {
        private readonly IAuthorRepository _authorRepository;

        public AuthorsController(IAuthorRepository authorRepository)
        {
            _authorRepository = authorRepository;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var authors = _authorRepository.GetAllAuthors();
            return Ok(authors);
        }

        [HttpPost]
        public IActionResult Create([FromBody] Author author)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _authorRepository.AddAuthor(author);
            return Ok(new { success = true, message = "Author created successfully!" });
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] Author author)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            author.AuthorId = id;
            _authorRepository.UpdateAuthor(author);
            return Ok(new { success = true, message = "Author updated successfully!" });
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            _authorRepository.DeleteAuthor(id);
            return Ok(new { success = true, message = "Author deleted successfully!" });
        }
    }
}
