using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Frontend.Models
{
    public class UserLogin
    {
        // Username for authentication
        public string Username { get; set; } = string.Empty;

        // Password for authentication
        public string Password { get; set; } = string.Empty;
    }

    public class AuthResponse
    {
        // JWT token received from API
        public string Token { get; set; } = string.Empty;
    }
}
