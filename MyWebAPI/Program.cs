using Microsoft.EntityFrameworkCore;
using MyWebAPI.Data;
using MyWebAPI.Services;

var builder = WebApplication.CreateBuilder(args);

// Add DbContext using SQLite
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// Register the service layer for Dependency Injection
builder.Services.AddScoped<IGameService, GameService>();
builder.Services.AddScoped<IPublisherService, PublisherService>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    // Enable OpenAPI documentation UI
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();