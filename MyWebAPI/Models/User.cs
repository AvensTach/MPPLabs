namespace MyWebAPI.Models
{
    public class User
    {
        // Primary key
        public int Id { get; set; }

        // Unique username
        public string Username { get; set; } = string.Empty;

        // Hashed password
        public string PasswordHash { get; set; } = string.Empty;

        // User role: "User" or "Admin"
        public string Role { get; set; } = "User";
    }
}