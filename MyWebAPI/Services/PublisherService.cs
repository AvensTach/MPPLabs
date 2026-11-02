using Microsoft.EntityFrameworkCore;
using MyWebAPI.Data;
using MyWebAPI.DTOs;
using MyWebAPI.Models;

namespace MyWebAPI.Services
{
    public class PublisherService : IPublisherService
    {
        private readonly AppDbContext _context;

        // Inject DbContext via constructor
        public PublisherService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<PublisherDto>> GetAllPublishersAsync()
        {
            // Map entities to DTOs
            return await _context.Publishers
                .Select(p => new PublisherDto { Id = p.Id, Name = p.Name })
                .ToListAsync();
        }

        public async Task<PublisherDto?> GetPublisherByIdAsync(int id)
        {
            var publisher = await _context.Publishers.FindAsync(id);
            if (publisher == null) return null;

            return new PublisherDto { Id = publisher.Id, Name = publisher.Name };
        }

        public async Task<PublisherDto> CreatePublisherAsync(CreateUpdatePublisherDto dto)
        {
            var publisher = new Publisher { Name = dto.Name };

            _context.Publishers.Add(publisher);
            await _context.SaveChangesAsync();

            return new PublisherDto { Id = publisher.Id, Name = publisher.Name };
        }

        public async Task<bool> UpdatePublisherAsync(int id, CreateUpdatePublisherDto dto)
        {
            var publisher = await _context.Publishers.FindAsync(id);
            if (publisher == null) return false;

            // Update entity properties
            publisher.Name = dto.Name;
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeletePublisherAsync(int id)
        {
            var publisher = await _context.Publishers.FindAsync(id);
            if (publisher == null) return false;

            _context.Publishers.Remove(publisher);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}