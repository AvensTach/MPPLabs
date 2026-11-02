using Microsoft.AspNetCore.Mvc;
using MyWebAPI.DTOs;
using MyWebAPI.Services;

namespace MyWebAPI.Controllers
{
    [ApiController]
    [Route("api/resources")] // Requirement 4: specific endpoint paths
    public class GamesController : ControllerBase
    {
        private readonly IGameService _gameService;

        // Inject GameService via constructor
        public GamesController(IGameService gameService)
        {
            _gameService = gameService;
        }

        // GET: api/resources
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GameDto>>> GetGames([FromQuery] string? genre, [FromQuery] string? search, [FromQuery] string? sort)
        {
            var games = await _gameService.GetAllGamesAsync(genre, search, sort);
            return Ok(games);
        }

        // GET: api/resources/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<GameDto>> GetGame(int id)
        {
            var game = await _gameService.GetGameByIdAsync(id);
            if (game == null) return NotFound(); // Return 404 if not found

            return Ok(game);
        }

        // POST: api/resources
        [HttpPost]
        public async Task<ActionResult<GameDto>> CreateGame(CreateUpdateGameDto gameDto)
        {
            // ModelState validity is checked automatically by [ApiController]
            var createdGame = await _gameService.CreateGameAsync(gameDto);

            // Return 201 Created with location header
            return CreatedAtAction(nameof(GetGame), new { id = createdGame.Id }, createdGame);
        }

        // PUT: api/resources/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateGame(int id, CreateUpdateGameDto gameDto)
        {
            var updated = await _gameService.UpdateGameAsync(id, gameDto);
            if (!updated) return NotFound(); // Return 404 if game to update does not exist

            // Return 204 No Content for successful update
            return NoContent();
        }

        // DELETE: api/resources/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGame(int id)
        {
            var deleted = await _gameService.DeleteGameAsync(id);
            if (!deleted) return NotFound();

            return NoContent();
        }
    }
}