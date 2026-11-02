namespace MyWebAPI.DTOs
{
    public class GameDto
    {
        // Unique identifier
        public int Id { get; set; }
        // Game title
        public string Title { get; set; } = string.Empty;
        // Game genre
        public string Genre { get; set; } = string.Empty;
        // Price value
        public decimal Price { get; set; }
        // Publisher name included for convenience
        public string PublisherName { get; set; } = string.Empty;
    }
}