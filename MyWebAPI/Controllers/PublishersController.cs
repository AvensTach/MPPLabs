using Microsoft.AspNetCore.Mvc;
using MyWebAPI.DTOs;
using MyWebAPI.Services;

namespace MyWebAPI.Controllers
{
    [ApiController]
    [Route("api/publishers")]
    public class PublishersController : ControllerBase
    {
        private readonly IPublisherService _publisherService;

        // Inject PublisherService via constructor
        public PublishersController(IPublisherService publisherService)
        {
            _publisherService = publisherService;
        }

        // GET: api/publishers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PublisherDto>>> GetPublishers()
        {
            var publishers = await _publisherService.GetAllPublishersAsync();
            return Ok(publishers);
        }

        // GET: api/publishers/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<PublisherDto>> GetPublisher(int id)
        {
            var publisher = await _publisherService.GetPublisherByIdAsync(id);
            if (publisher == null) return NotFound();

            return Ok(publisher);
        }

        // POST: api/publishers
        [HttpPost]
        public async Task<ActionResult<PublisherDto>> CreatePublisher(CreateUpdatePublisherDto dto)
        {
            var created = await _publisherService.CreatePublisherAsync(dto);

            // Return 201 Created with location header
            return CreatedAtAction(nameof(GetPublisher), new { id = created.Id }, created);
        }

        // PUT: api/publishers/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePublisher(int id, CreateUpdatePublisherDto dto)
        {
            var updated = await _publisherService.UpdatePublisherAsync(id, dto);
            if (!updated) return NotFound();

            return NoContent();
        }

        // DELETE: api/publishers/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePublisher(int id)
        {
            var deleted = await _publisherService.DeletePublisherAsync(id);
            if (!deleted) return NotFound();

            return NoContent();
        }
    }
}