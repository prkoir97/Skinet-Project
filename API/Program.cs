using Core.Interfaces;
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
builder.Services.AddScoped<IProductRepository, ProductRepository>(); // AddScoped means that a new instance of ProductRepository will be created for each scope of the application's request pipeline. Erases after.
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
// End Container

var app = builder.Build();

// Middleware: software that we can use to do something with that request on it's journey in, and out

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) 
{
    app.UseSwagger();               // Swagger documents our API endpoints
    app.UseSwaggerUI();
}

app.UseStaticFiles();

app.UseAuthorization();

app.MapControllers(); // Where API sends HTTP requests (registers controller endpoints)

using var scope = app.Services.CreateScope(); // Create a scoped service container scope for managing service lifetimes within a boundary
var services = scope.ServiceProvider; // Obtain the service provider from the created scope
var context = services.GetRequiredService<StoreContext>(); // Retrieve the StoreContext service from the service provider
var logger = services.GetRequiredService<ILogger<Program>>(); // Retrieve the ILogger service with the Program category from the service provider

try
{
    await context.Database.MigrateAsync(); // Attempt to migrate the database asynchronously using the StoreContext
    await StoreContextSeed.SeedAsync(context);
}
catch (Exception ex)
{
    logger.LogError(ex, "An error occurred during migration"); // Log any exceptions that occur during migration
}


app.Run();