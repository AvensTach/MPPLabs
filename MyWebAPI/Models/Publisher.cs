namespace MyWebAPI.Models
{
    public class Publisher
    {
        // Primary key
        public int Id { get; set; }
        
        // Name of the publisher
        public string Name { get; set; } = string.Empty;
        
        // Navigation property for one-to-many relationship
        public ICollection<Game> Games { get; set; } = new List<Game>();
    }
}