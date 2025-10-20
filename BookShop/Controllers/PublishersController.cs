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
        public IActionResult GetAll()
        {
            var publishers = _publisherRepository.GetAllPublishers();
            return Ok(publishers);
        }

        [HttpPost]
        public IActionResult Create([FromBody] Publisher publisher)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _publisherRepository.AddPublisher(publisher);
            return Ok(new { success = true, message = "Publisher created successfully!" });
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] Publisher publisher)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            publisher.PublisherId = id;
            _publisherRepository.UpdatePublisher(publisher);
            return Ok(new { success = true, message = "Publisher updated successfully!" });
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            _publisherRepository.DeletePublisher(id);
            return Ok(new { success = true, message = "Publisher deleted successfully!" });
        }
    }
}
