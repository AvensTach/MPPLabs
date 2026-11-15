using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Frontend.Models;

namespace Frontend.Services
{
    public interface IApiService
    {
        // Authenticates user and stores JWT token
        Task<bool> LoginAsync(string username, string password);

        // Clears the stored JWT token
        void Logout();

        // Retrieves all games from the API
        Task<IEnumerable<Game>> GetGamesAsync();

        // Deletes a game by ID
        Task<bool> DeleteGameAsync(int id);

        Task<bool> CreateGameAsync(CreateGameDto game);

        Task<bool> UpdateGameAsync(int id, CreateGameDto game);

        Task<IEnumerable<Publisher>> GetPublishersAsync();

        Task<bool> RegisterAsync(string username, string password);
    }
}
