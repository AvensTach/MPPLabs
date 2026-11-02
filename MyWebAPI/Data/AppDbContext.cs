using MyWebAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace MyWebAPI.API.Data
{
    public class AppDbContext : DbContext
    {
        // Constructor accepting DbContextOptions
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        // DbSets representing database tables
        public DbSet<Game> Games { get; set; }
        public DbSet<Publisher> Publishers { get; set; }
    }
}