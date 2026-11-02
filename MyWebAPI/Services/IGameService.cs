using MyWebAPI.DTOs;

namespace MyWebAPI.Services
{
    public interface IGameService
    {
        // Retrieves games with optional filtering, searching, and sorting
        Task<IEnumerable<GameDto>> GetAllGamesAsync(string? genre, string? searchTerm, string? sortBy);

        // Retrieves a single game by ID
        Task<GameDto?> GetGameByIdAsync(int id);

        // Creates a new game
        Task<GameDto> CreateGameAsync(CreateUpdateGameDto gameDto, int ownerId);

        // Updates an existing game
        Task<bool> UpdateGameAsync(int id, CreateUpdateGameDto gameDto, int currentUserId, string role);
        // Deletes a game by ID
        Task<bool> DeleteGameAsync(int id, int currentUserId, string role);
    }
}