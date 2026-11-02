using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyWebAPI.DTOs;
using MyWebAPI.Services;
using System.Security.Claims;

namespace MyWebAPI.Controllers
{
    [ApiController]
    [Route("api/resources")]
    [Authorize] // Require JWT for all endpoints in this controller
    public class GamesController : ControllerBase
    {
        private readonly IGameService _gameService;

        public GamesController(IGameService gameService)
        {
            _gameService = gameService;
        }

        // Extract user information from JWT claims
        private int GetCurrentUserId() => int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "0");
        private string GetCurrentUserRole() => User.FindFirstValue(ClaimTypes.Role) ?? "User";

        [HttpGet]
        [AllowAnonymous] // Allow anyone to read the list
        public async Task<ActionResult<IEnumerable<GameDto>>> GetGames([FromQuery] string? genre, [FromQuery] string? search, [FromQuery] string? sort)
        {
            var games = await _gameService.GetAllGamesAsync(genre, search, sort);
            return Ok(games);
        }

        [HttpGet("{id}")]
        [AllowAnonymous] // Allow anyone to read a single resource
        public async Task<ActionResult<GameDto>> GetGame(int id)
        {
            var game = await _gameService.GetGameByIdAsync(id);
            if (game == null) return NotFound();

            return Ok(game);
        }

        [HttpPost]
        public async Task<ActionResult<GameDto>> CreateGame(CreateUpdateGameDto gameDto)
        {
            var createdGame = await _gameService.CreateGameAsync(gameDto, GetCurrentUserId());
            return CreatedAtAction(nameof(GetGame), new { id = createdGame.Id }, createdGame);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateGame(int id, CreateUpdateGameDto gameDto)
        {
            try
            {
                var updated = await _gameService.UpdateGameAsync(id, gameDto, GetCurrentUserId(), GetCurrentUserRole());
                if (!updated) return NotFound();

                return NoContent();
            }
            catch (UnauthorizedAccessException)
            {
                // Returns 403 Forbidden if user tries to update someone else's resource
                return Forbid();
            }
        }

        [HttpDelete("{id}")]
        // Example of Policy-based authorization combined with manual resource checking
        [Authorize(Policy = "RequireAdminRole")] // Uncomment if ONLY admins can delete
        public async Task<IActionResult> DeleteGame(int id)
        {
            try
            {
                var deleted = await _gameService.DeleteGameAsync(id, GetCurrentUserId(), GetCurrentUserRole());
                if (!deleted) return NotFound();

                return NoContent();
            }
            catch (UnauthorizedAccessException)
            {
                return Forbid();
            }
        }
    }
}