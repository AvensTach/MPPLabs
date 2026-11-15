// Models/CreateGameDto.cs
namespace Frontend.Models
{
    public class CreateGameDto
    {
        public string Title { get; set; } = string.Empty;
        public string Genre { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int PublisherId { get; set; }
    }
}