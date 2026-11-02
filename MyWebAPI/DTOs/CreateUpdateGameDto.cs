using System.ComponentModel.DataAnnotations;

namespace MyWebAPI.DTOs
{
    public class CreateUpdateGameDto
    {
        // Title is required with max length of 100
        [Required]
        [MaxLength(100)]
        public string Title { get; set; } = string.Empty;

        // Genre is required
        [Required]
        [MaxLength(50)]
        public string Genre { get; set; } = string.Empty;

        // Price must be a positive number
        [Range(0.01, 1000)]
        public decimal Price { get; set; }

        // Valid Publisher ID is required
        [Required]
        public int PublisherId { get; set; }
    }
}