using Microsoft.EntityFrameworkCore;
using MyWebAPI.Data;
using MyWebAPI.DTOs;
using MyWebAPI.Models;

namespace MyWebAPI.Services
{
    public class GameService : IGameService
    {
        private readonly AppDbContext _context;

        // Inject DbContext via constructor
        public GameService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<GameDto>> GetAllGamesAsync(string? genre, string? searchTerm, string? sortBy)
        {
            // Start querying the database
            var query = _context.Games.Include(g => g.Publisher).AsQueryable();

            // Apply filtering by genre
            if (!string.IsNullOrWhiteSpace(genre))
            {
                query = query.Where(g => g.Genre.ToLower() == genre.ToLower());
            }

            // Apply searching by title
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                query = query.Where(g => g.Title.ToLower().Contains(searchTerm.ToLower()));
            }

            // Apply sorting
            query = sortBy?.ToLower() switch
            {
                "price_desc" => query.OrderByDescending(g => (double)g.Price),
                "price" => query.OrderBy(g => (double)g.Price),
                "title_desc" => query.OrderByDescending(g => g.Title),
                _ => query.OrderBy(g => g.Title) // Default sort
            };

            // Map entities to DTOs
            return await query.Select(g => new GameDto
            {
                Id = g.Id,
                Title = g.Title,
                Genre = g.Genre,
                Price = g.Price,
                PublisherName = g.Publisher != null ? g.Publisher.Name : "Unknown"
            }).ToListAsync();
        }

        public async Task<GameDto?> GetGameByIdAsync(int id)
        {
            // Find game and map to DTO
            var game = await _context.Games.Include(g => g.Publisher)
                                           .FirstOrDefaultAsync(g => g.Id == id);

            if (game == null) return null;

            return new GameDto
            {
                Id = game.Id,
                Title = game.Title,
                Genre = game.Genre,
                Price = game.Price,
                PublisherName = game.Publisher != null ? game.Publisher.Name : "Unknown"
            };
        }

        public async Task<GameDto> CreateGameAsync(CreateUpdateGameDto gameDto, int ownerId)
        {
            var game = new Game
            {
                Title = gameDto.Title,
                Genre = gameDto.Genre,
                Price = gameDto.Price,
                PublisherId = gameDto.PublisherId,
                OwnerId = ownerId // Assign owner
            };

            _context.Games.Add(game);
            await _context.SaveChangesAsync();

            return await GetGameByIdAsync(game.Id) ?? throw new InvalidOperationException();
        }

        public async Task<bool> UpdateGameAsync(int id, CreateUpdateGameDto gameDto, int currentUserId, string role)
        {
            var game = await _context.Games.FindAsync(id);
            if (game == null) return false;

            // Resource protection: Only Owner or Admin can update
            if (game.OwnerId != currentUserId && role != "Admin")
                throw new UnauthorizedAccessException();

            game.Title = gameDto.Title;
            game.Genre = gameDto.Genre;
            game.Price = gameDto.Price;
            game.PublisherId = gameDto.PublisherId;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteGameAsync(int id, int currentUserId, string role)
        {
            var game = await _context.Games.FindAsync(id);
            if (game == null) return false;

            // Resource protection: Only Owner or Admin can delete
            if (game.OwnerId != currentUserId && role != "Admin")
                throw new UnauthorizedAccessException();

            _context.Games.Remove(game);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}