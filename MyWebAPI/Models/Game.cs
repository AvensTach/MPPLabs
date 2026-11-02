namespace MyWebAPI.Models
{
    public class Game
    {
        // Primary key
        public int Id { get; set; }

        // Title of the game
        public string Title { get; set; } = string.Empty;

        // Genre of the game
        public string Genre { get; set; } = string.Empty;

        // Game price
        public decimal Price { get; set; }

        // Foreign key to Publisher
        public int PublisherId { get; set; }

        // Navigation property
        public Publisher? Publisher { get; set; }
    }
}