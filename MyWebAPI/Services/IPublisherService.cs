using MyWebAPI.DTOs;

namespace MyWebAPI.Services
{
    public interface IPublisherService
    {
        // Retrieves all publishers
        Task<IEnumerable<PublisherDto>> GetAllPublishersAsync();

        // Retrieves a single publisher by ID
        Task<PublisherDto?> GetPublisherByIdAsync(int id);

        // Creates a new publisher
        Task<PublisherDto> CreatePublisherAsync(CreateUpdatePublisherDto dto);

        // Updates an existing publisher
        Task<bool> UpdatePublisherAsync(int id, CreateUpdatePublisherDto dto);

        // Deletes a publisher by ID
        Task<bool> DeletePublisherAsync(int id);
    }
}