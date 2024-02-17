using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args); // Configures kestrel web server, resposible for running our web app

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<StoreContext>(opt => 
{
opt. UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"));
});

// End Container

var app = builder.Build();

// Middleware: software that we can use to do something with that request on it's journey in, and out

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) 
{
    app.UseSwagger();               // Swagger documents our API endpoints
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers(); // Where API sends HTTP requests (registers controller endpoints)

app.Run();