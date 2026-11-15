using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Frontend.Models
{
    public class Game
    {
        // Unique identifier
        public int Id { get; set; }

        // Game title
        public string Title { get; set; } = string.Empty;

        // Game genre
        public string Genre { get; set; } = string.Empty;

        // Game price
        public decimal Price { get; set; }

        // ID of the publisher
        public int PublisherId { get; set; }

        // Name of the publisher
        public string PublisherName { get; set; } = string.Empty;
    }
}
