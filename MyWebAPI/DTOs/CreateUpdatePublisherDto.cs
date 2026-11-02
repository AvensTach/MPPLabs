using System.ComponentModel.DataAnnotations;

namespace MyWebAPI.DTOs
{
    public class CreateUpdatePublisherDto
    {
        // Name is required with a maximum length
        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;
    }
}