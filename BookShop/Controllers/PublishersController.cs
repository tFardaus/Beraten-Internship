using BookShop.Models;
using BookShop.Services;
using Microsoft.AspNetCore.Mvc;

namespace BookShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PublishersController : ControllerBase
    {
        private readonly IPublisherRepository _publisherRepository;

        public PublishersController(IPublisherRepository publisherRepository)
        {
            _publisherRepository = publisherRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var publishers = await _publisherRepository.GetAllPublishersAsync();
            return Ok(publishers);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Publisher publisher)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _publisherRepository.AddPublisherAsync(publisher);
            return Ok(new { success = true, message = "Publisher created successfully!" });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Publisher publisher)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            publisher.PublisherId = id;
            await _publisherRepository.UpdatePublisherAsync(publisher);
            return Ok(new { success = true, message = "Publisher updated successfully!" });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _publisherRepository.DeletePublisherAsync(id);
            return Ok(new { success = true, message = "Publisher deleted successfully!" });
        }
    }
}
